using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnPointStruct[] enemySpawnPoints;
    [SerializeField] private GameObject[] enemyPrefabs; 
    private GameObject player; 

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
    }

    public List<EnemyBaseClass> SpawnEnemy(int enemiesToSpawn)
    {
        Array.Sort(enemySpawnPoints, (a, b) =>
        {
            float distanceA = Vector3.Distance(player.transform.position, a.transform.position);
            float distanceB = Vector3.Distance(player.transform.position, b.transform.position);

            return distanceA.CompareTo(distanceB);
        });

        List<EnemyBaseClass> newEnemies = new(); 
        int j = 0;
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            if(j >= enemySpawnPoints.Length) j = 0;

            int roll = UnityEngine.Random.Range(0, enemyPrefabs.Length);
            GameObject newEnemy = Instantiate(enemyPrefabs[roll], enemySpawnPoints[j].transform.position, Quaternion.identity);
            newEnemies.Add(newEnemy.GetComponent<EnemyBaseClass>());
            j++;
        }

        return newEnemies;
    }
}
