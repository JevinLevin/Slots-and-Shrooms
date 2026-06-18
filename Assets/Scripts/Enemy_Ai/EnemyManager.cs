using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemySpawner spawner;
    private List<EnemyBaseClass> spawnedEnemies = new();

    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.onEnemyDies += OnEnemyDies;
    }

    private void OnEnemyDies() => SpawnEnemies(1); 
    public void StartWave(int amount) => SpawnEnemies(amount);

    public void EndWave()
    {
        // Kill all enemies
        Debug.Log("DESPAWNENEMIES"); 
        foreach (var enemy in spawnedEnemies)
            if(enemy)
                enemy.Despawn();
        spawnedEnemies.Clear();
    }

    private void SpawnEnemies(int amount)
    {
        List<EnemyBaseClass> newEnemies = spawner.SpawnEnemy(amount);
        foreach (EnemyBaseClass newEnemy in newEnemies) { spawnedEnemies.Add(newEnemy); }
    }
}
