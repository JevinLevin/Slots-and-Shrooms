using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] EnemySpawner spawner;
    private List<EnemyBaseClass> spawnedEnemies = new();

    public static EnemyManager Instance { get; private set; }

    private bool shouldSpawn;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.onEnemyDies += OnEnemyDies;
    }

    private void OnEnemyDies() => SpawnEnemies(1); 
    public void StartWave(int amount)
    {
        shouldSpawn = true;
        SpawnEnemies(amount);
    }

    public void EndWave()
    {
        shouldSpawn = false;
        // Kill all enemies
        Debug.Log("DESPAWNENEMIES"); 
        foreach (var enemy in spawnedEnemies)
            if(enemy)
                enemy.Despawn();
        spawnedEnemies.Clear();
    }

    private void SpawnEnemies(int amount)
    {
        if (!shouldSpawn)
            return;
        
        List<EnemyBaseClass> newEnemies = spawner.SpawnEnemy(amount);
        foreach (EnemyBaseClass newEnemy in newEnemies) { spawnedEnemies.Add(newEnemy); }
    }
}
