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
        Transform[] allowedEnemies = new Transform[allowedEnemiesID.Length];
        for (int i = 0; i < allowedEnemiesID.Length; i++)
            allowedEnemies[i] = enemyPrefabs[allowedEnemiesID[i]];

        int waveTotal = Random.Range(minEnemies, maxEnemies + 1);
        List<Transform> spawnList = new List<Transform>();

        float[] InverseCosts = new float[allowedEnemies.Length];
        float totalInverse = 0;

        foreach (Transform enemy in allowedEnemies)
        {
            float inverseCost = 100 - enemy.GetComponent<EnemyManager>().spawnCost;
            InverseCosts[System.Array.IndexOf(allowedEnemies, enemy)] = inverseCost;
            totalInverse += inverseCost;
        }

        float[] spawnPercentages = new float[allowedEnemies.Length];
        for (int i = 0; i < allowedEnemies.Length; i++)
            spawnPercentages[i] = InverseCosts[i] / totalInverse;

        for (int i = 0; i < allowedEnemies.Length; i++)
        {
            int count = Mathf.RoundToInt(spawnPercentages[i] * waveTotal);
            for (int j = 0; j < count; j++)
                spawnList.Add(allowedEnemies[i]);
        }
        for (int i = spawnList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Transform t = spawnList[i];
            spawnList[i] = spawnList[j];
            spawnList[j] = t;
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