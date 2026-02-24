using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class WorldSpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;

    public Transform player;

    [Header("Chunk Settings")]
    public int chunkSize = 20;
    public int enemiesPerChunk = 3;     // spawn mỗi chunk
    public int renderDistance = 1;      // 1 = 3x3 chunk

    [Header("Spawn Settings")]
    public float minDistanceFromPlayer = 5f;
    public float navMeshSearchRadius = 3f;

    [Header("Mob Limit")]
    public int maxTotalEnemies = 20;    // GIỚI HẠN TỔNG QUÁI

    private Vector2Int currentChunk;
    private Dictionary<Vector2Int, List<GameObject>> activeChunks =
        new Dictionary<Vector2Int, List<GameObject>>();

    public bool canSpawn = false; // ban đầu tắt

    void Start()
    {
        currentChunk = GetPlayerChunk();

        if (canSpawn)
            UpdateChunks();
    }

    void Update()
    {
        if (!canSpawn) return;

        Vector2Int newChunk = GetPlayerChunk();

        if (newChunk != currentChunk)
        {
            currentChunk = newChunk;
            UpdateChunks();
        }
    }

    Vector2Int GetPlayerChunk()
    {
        int chunkX = Mathf.FloorToInt(player.position.x / chunkSize);
        int chunkZ = Mathf.FloorToInt(player.position.z / chunkSize);
        return new Vector2Int(chunkX, chunkZ);
    }

    void UpdateChunks()
    {
        HashSet<Vector2Int> neededChunks = new HashSet<Vector2Int>();

        // Tính các chunk cần giữ (3x3)
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector2Int chunk = new Vector2Int(currentChunk.x + x, currentChunk.y + z);
                neededChunks.Add(chunk);

                if (!activeChunks.ContainsKey(chunk))
                {
                    SpawnChunk(chunk);
                }
            }
        }

        // Xoá chunk không còn trong phạm vi
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
            GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);

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

            Vector3 randomPoint = new Vector3(randomX, player.position.y, randomZ);

            if (NavMesh.SamplePosition(randomPoint, out hit, navMeshSearchRadius, NavMesh.AllAreas))
            {
                if (Vector3.Distance(hit.position, player.position) >= minDistanceFromPlayer)
                {
                    return hit.position;
                }
            }
        }

        return player.position + Vector3.forward * minDistanceFromPlayer;
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
        currentChunk = GetPlayerChunk();
        UpdateChunks();
    }
}