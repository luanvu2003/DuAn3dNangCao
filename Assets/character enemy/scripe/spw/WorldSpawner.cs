using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class WorldSpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Players")]
    public Transform[] players;

    [Header("Chunk Settings")]
    public int chunkSize = 20;
    public int enemiesPerChunk = 3;
    public int renderDistance = 2; // tăng lên 2-3 cho giống Minecraft

    [Header("Spawn Settings")]
    public float minDistanceFromPlayer = 5f;

    [Header("Mob Limit")]
    public int maxTotalEnemies = 50;

    private Dictionary<Vector2Int, List<GameObject>> activeChunks =
        new Dictionary<Vector2Int, List<GameObject>>();

    // 🔥 lưu tất cả mob đã spawn theo chunk (không bị mất)
    private Dictionary<Vector2Int, List<GameObject>> allSpawnedChunks =
        new Dictionary<Vector2Int, List<GameObject>>();

    public bool canSpawn = false;

    void Start()
    {
        if (canSpawn)
            UpdateChunks();
    }

    void Update()
    {
        if (!canSpawn) return;

        UpdateChunks();
    }

    // =========================
    // CHUNK SYSTEM
    // =========================

    void UpdateChunks()
    {
        HashSet<Vector2Int> neededChunks = new HashSet<Vector2Int>();

        foreach (Transform p in players)
        {
            if (p == null || !p.gameObject.activeInHierarchy)
                continue;

            Vector2Int playerChunk = GetChunkFromPosition(p.position);

            for (int x = -renderDistance; x <= renderDistance; x++)
            {
                for (int z = -renderDistance; z <= renderDistance; z++)
                {
                    Vector2Int chunk =
                        new Vector2Int(playerChunk.x + x, playerChunk.y + z);

                    neededChunks.Add(chunk);

                    if (!activeChunks.ContainsKey(chunk))
                    {
                        if (allSpawnedChunks.ContainsKey(chunk))
                        {
                            ReactivateChunk(chunk);
                        }
                        else
                        {
                            SpawnChunk(chunk);
                        }
                    }
                }
            }
        }

        List<Vector2Int> chunksToRemove = new List<Vector2Int>();

        foreach (var chunk in activeChunks.Keys)
        {
            if (!neededChunks.Contains(chunk))
            {
                chunksToRemove.Add(chunk);
            }
        }

        foreach (var chunk in chunksToRemove)
        {
            UnloadChunk(chunk);
        }
    }

    Vector2Int GetChunkFromPosition(Vector3 position)
    {
        int chunkX = Mathf.FloorToInt(position.x / chunkSize);
        int chunkZ = Mathf.FloorToInt(position.z / chunkSize);
        return new Vector2Int(chunkX, chunkZ);
    }

    // =========================
    // SPAWN
    // =========================

    void SpawnChunk(Vector2Int chunkCoord)
    {
        if (GetTotalEnemyCount() >= maxTotalEnemies)
            return;

        List<GameObject> spawnedEnemies = new List<GameObject>();

        for (int i = 0; i < enemiesPerChunk; i++)
        {
            if (GetTotalEnemyCount() >= maxTotalEnemies)
                break;

            Vector3 spawnPos = GetRandomPositionInChunk(chunkCoord);
            if (spawnPos == Vector3.zero) continue;

            int randomIndex = Random.Range(0, enemyPrefabs.Length);

            GameObject enemy =
                Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);

            spawnedEnemies.Add(enemy);
        }

        activeChunks.Add(chunkCoord, spawnedEnemies);
        allSpawnedChunks.Add(chunkCoord, spawnedEnemies);
    }

    void UnloadChunk(Vector2Int chunkCoord)
    {
        if (activeChunks.ContainsKey(chunkCoord))
        {
            foreach (GameObject enemy in activeChunks[chunkCoord])
            {
                if (enemy != null)
                    enemy.SetActive(false); // 🔥 không destroy
            }

            activeChunks.Remove(chunkCoord);
        }
    }

    void ReactivateChunk(Vector2Int chunkCoord)
    {
        List<GameObject> enemies = allSpawnedChunks[chunkCoord];

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                enemy.SetActive(true);
        }

        activeChunks.Add(chunkCoord, enemies);
    }

    Vector3 GetRandomPositionInChunk(Vector2Int chunkCoord)
    {
        float startX = chunkCoord.x * chunkSize;
        float startZ = chunkCoord.y * chunkSize;

        for (int attempt = 0; attempt < 25; attempt++)
        {
            float randomX = Random.Range(startX, startX + chunkSize);
            float randomZ = Random.Range(startZ, startZ + chunkSize);

            Vector3 randomPoint = new Vector3(randomX, 100f, randomZ);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 100f, NavMesh.AllAreas))
            {
                bool tooClose = false;

                foreach (Transform p in players)
                {
                    if (p == null) continue;

                    if (Vector3.Distance(hit.position, p.position) < minDistanceFromPlayer)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                    return hit.position;
            }
        }

        return Vector3.zero;
    }

    int GetTotalEnemyCount()
    {
        int count = 0;

        foreach (var chunk in activeChunks.Values)
        {
            foreach (GameObject enemy in chunk)
            {
                if (enemy != null && enemy.activeInHierarchy)
                    count++;
            }
        }

        return count;
    }

    public void StartSpawning()
    {
        if (canSpawn) return;

        canSpawn = true;
        UpdateChunks();
    }

    public void IncreaseSpawnAmount(int amount)
    {
        enemiesPerChunk += amount;
        Debug.Log("Enemies per chunk tăng lên: " + enemiesPerChunk);
    }
}