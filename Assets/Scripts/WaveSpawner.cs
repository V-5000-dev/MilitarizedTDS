using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] enemyPrefabs;
    public Material enemyMaterials;
    public int currentWave;
    public EnemyManager enemyManager;
    void SpawnWave(int minEnemies, int maxEnemies, Transform[] allowedEnemies)
    {
        int index = 0;
        int totalSpawned = 0;
        bool startSpawn = false;
        float averageCost = 0;
        int[] spawnCosts = new int[allowedEnemies.Length];
        foreach (Transform enemy in allowedEnemies)
        {
            enemyManager = enemy.GetComponent<EnemyManager>();
            averageCost += enemyManager.spawnCost;

        }
        averageCost /= allowedEnemies.Length;
        foreach (Transform enemy in allowedEnemies)
        {
            enemyManager = enemy.GetComponent<EnemyManager>();
            float spawnChance = Mathf.Clamp(averageCost / enemyManager.spawnCost, 0.1f, 1f;)
        }
    }
}
