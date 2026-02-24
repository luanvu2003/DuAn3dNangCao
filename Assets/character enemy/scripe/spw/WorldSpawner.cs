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
    public int renderDistance = 1;

    [Header("Spawn Settings")]
    public float minDistanceFromPlayer = 5f;
    public float navMeshSearchRadius = 3f;

    [Header("Mob Limit")]
    public int maxTotalEnemies = 20;

    private Dictionary<Vector2Int, List<GameObject>> activeChunks =
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
                        SpawnChunk(chunk);
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
            DespawnChunk(chunk);
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

            int randomIndex = Random.Range(0, enemyPrefabs.Length);

            GameObject enemy =
                Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);

            spawnedEnemies.Add(enemy);
        }

        activeChunks.Add(chunkCoord, spawnedEnemies);
    }

    void DespawnChunk(Vector2Int chunkCoord)
    {
        if (activeChunks.ContainsKey(chunkCoord))
        {
            foreach (GameObject enemy in activeChunks[chunkCoord])
            {
                if (enemy != null)
                    Destroy(enemy);
            }

            activeChunks.Remove(chunkCoord);
        }
    }

    Vector3 GetRandomPositionInChunk(Vector2Int chunkCoord)
    {
        float startX = chunkCoord.x * chunkSize;
        float startZ = chunkCoord.y * chunkSize;

        NavMeshHit hit;

        for (int attempt = 0; attempt < 25; attempt++)
        {
            float randomX = Random.Range(startX, startX + chunkSize);
            float randomZ = Random.Range(startZ, startZ + chunkSize);

            // Bắt đầu từ cao hơn player để chắc chắn nằm trên map
            float sampleY = 200f;

            foreach (Transform p in players)
            {
                if (p != null && p.gameObject.activeInHierarchy)
                {
                    sampleY = p.position.y + 50f;
                    break;
                }
            }

            Vector3 randomPoint = new Vector3(randomX, sampleY, randomZ);

            if (NavMesh.SamplePosition(randomPoint, out hit, 100f, NavMesh.AllAreas))
            {
                bool tooClose = false;

                foreach (Transform p in players)
                {
                    if (p == null || !p.gameObject.activeInHierarchy)
                        continue;

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

        // Nếu fail thì KHÔNG spawn
        return new Vector3(float.MinValue, float.MinValue, float.MinValue);
    }

    int GetTotalEnemyCount()
    {
        int count = 0;

        foreach (var chunk in activeChunks.Values)
        {
            count += chunk.Count;
        }

        return count;
    }

    public void StartSpawning()
    {
        if (canSpawn) return;

        canSpawn = true;
        UpdateChunks();
    }
}