using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] enemyPrefabs;
    public Material enemyMaterials;
    public int currentWave;
    public EnemyManager enemyManager;
    public List<Transform> SpawnWave(int minEnemies, int maxEnemies, Transform[] allowedEnemies, float waveBudget)
    {
        int waveTotal = Random.Range(minEnemies, maxEnemies + 1);
        List<Transform> spawnList = new List<Transform>();

        List<Transform> sorted = new List<Transform>();
        List<Transform> remaining = new List<Transform>(allowedEnemies);
        while (remaining.Count > 0)
        {
            int index = 0;
            for(int i = 1; i < remaining.Count; i++)
            {
                if(remaining[i].GetComponent<EnemyManager>().spawnCost > remaining[index].GetComponent<EnemyManager>().spawnCost)
                    index++;
            }
            sorted.Add(remaining[index]);
            remaining.RemoveAt(index);
        }
        float remainingBudget = waveBudget;
        while(spawnList.Count < waveTotal && remainingBudget > 0)
        {
            List<Transform> weightedPool = new List<Transform>();
            for (int i = weightedPool.co)
        }

  

        



    }

}
