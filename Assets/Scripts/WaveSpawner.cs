using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] enemyPrefabs;
    public int[] allowedEnemies;
    public float spawnDelay;
    public Transform spawnPoint;

    public List<Transform> GenerateWave(int minEnemies, int maxEnemies, int[] allowedEnemiesID, float waveBudget)
    {
        // Build allowed list from indices
        Transform[] allowed = new Transform[allowedEnemiesID.Length];
        for (int i = 0; i < allowedEnemiesID.Length; i++)
            allowed[i] = enemyPrefabs[allowedEnemiesID[i]];

        int waveTotal = Random.Range(minEnemies, maxEnemies + 1);
        List<Transform> spawnList = new List<Transform>();

        // Sort most to least expensive
        List<Transform> sorted = new List<Transform>();
        List<Transform> remaining = new List<Transform>(allowed);

        while (remaining.Count > 0)
        {
            int index = 0;
            for (int i = 1; i < remaining.Count; i++)
            {
                if (remaining[i].GetComponent<EnemyManager>().spawnCost > remaining[index].GetComponent<EnemyManager>().spawnCost)
                    index = i;
            }
            sorted.Add(remaining[index]);
            remaining.RemoveAt(index);
        }

        float remainingBudget = waveBudget;

        while (spawnList.Count < waveTotal && remainingBudget > 0)
        {
            // Calculate total weight — cheapest (last in sorted) gets highest weight
            int totalWeight = 0;
            for (int i = 0; i < sorted.Count; i++)
                totalWeight += (i + 1);

            // Roll and pick enemy based on weight
            int roll = Random.Range(0, totalWeight);
            int cumulative = 0;
            Transform enemy = null;

            for (int i = sorted.Count - 1; i >= 0; i--)
            {
                cumulative += (i + 1);
                if (roll < cumulative)
                {
                    enemy = sorted[i];
                    break;
                }
            }

            // If we can't afford the picked enemy, skip it
            if (enemy == null || enemy.GetComponent<EnemyManager>().spawnCost > remainingBudget)
                break;

            spawnList.Add(enemy);
            remainingBudget -= enemy.GetComponent<EnemyManager>().spawnCost;
        }

        return spawnList;
    }

    public IEnumerator SpawnWave(List<Transform> spawnList)
    {
        foreach (Transform enemy in spawnList)
        {
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void Start()
    {
        List<Transform> wave = GenerateWave(10, 20, allowedEnemies, 100);
        StartCoroutine(SpawnWave(wave));
    }
}