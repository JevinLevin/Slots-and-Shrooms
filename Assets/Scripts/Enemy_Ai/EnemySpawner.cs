using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Vector2 spawnRadius = new Vector2(10, 25);
    private GameObject player; 

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
    }

    public List<EnemyBaseClass> SpawnEnemy(int enemiesToSpawn)
    {
        

        List<EnemyBaseClass> newEnemies = new(); 
        int j = 0;
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            // Random nav mesh position in range
            int failSafe = 20;
            Vector3 randomPos = player.transform.position;
            while (failSafe > 0)
            {
                failSafe--;
                randomPos = GetRandomPoint(player.transform.position, spawnRadius.y);

                if (Vector3.Distance(player.transform.position, randomPos) < spawnRadius.x)
                    continue;
            }


            int roll = UnityEngine.Random.Range(0, enemyPrefabs.Length);
            GameObject newEnemy = Instantiate(enemyPrefabs[roll], randomPos, Quaternion.identity);
            newEnemies.Add(newEnemy.GetComponent<EnemyBaseClass>());
            j++;
        }

        return newEnemies;
    }

    public Vector3 GetRandomPoint(Vector3 center, float maxDistance)
    {
        // Get Random Point inside Sphere which position is center, radius is maxDistance
        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * maxDistance + center;

        NavMeshHit hit; // NavMesh Sampling Info Container

        // from randomPos find a nearest point on NavMesh surface in range of maxDistance
        NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

        return hit.position;
    }
}
