using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class WaveEnemies
{
    public int[] values;
}

public class WaveSpawner : MonoBehaviour
{
    public Transform[] enemyPrefabs;
    public int wave;
    public int[] waveBudget;
    public int[] enemyCosts;
    public WaveEnemies[] waveEnemiesArray;
    public float spawnDelay;
    public Transform spawnPoint;

    public List<Transform> GenerateWave(int[] allowedEnemiesID, float waveBudget)
    {
        float[] weights = new float[allowedEnemiesID.Length];
        float totalWeight = 0f;
        for (int i = 0; i < allowedEnemiesID.Length; i++)
        {
            weights[i] = 1f / enemyCosts[allowedEnemiesID[i]];
            totalWeight += weights[i];
        }

        int minCost = allowedEnemiesID.Min(id => enemyCosts[id]);
        List<Transform> spawnList = new List<Transform>();
        float remainingBudget = waveBudget;

        while (remainingBudget >= minCost)
        {
            float roll = Random.Range(0f, totalWeight);
            float cumulative = 0f;
            for (int i = 0; i < allowedEnemiesID.Length; i++)
            {
                cumulative += weights[i];
                if (roll <= cumulative)
                {
                    int id = allowedEnemiesID[i];
                    if (enemyCosts[id] <= remainingBudget)
                    {
                        spawnList.Add(enemyPrefabs[id]);
                        remainingBudget -= enemyCosts[id];
                    }
                    break;
                }
            }
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
        wave = 0;
    }
    public void onNextWaveButton()
    {
        List<Transform> spawnedWave = GenerateWave(waveEnemiesArray[wave].values, waveBudget[wave]);
        StartCoroutine(SpawnWave(spawnedWave));
        wave++;
    }
}