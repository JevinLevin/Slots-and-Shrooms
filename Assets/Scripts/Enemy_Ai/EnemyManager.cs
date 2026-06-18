using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyManager : MonoBehaviour
{

    private List<EnemyBaseClass> spawnedEnemies = new();

    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // FOR TESTING PURPOSES STORE ALL CURRENT ENEMIES IN THE SCENE ON START
        spawnedEnemies = FindObjectsByType<EnemyBaseClass>(sortMode: FindObjectsSortMode.None).ToList();
        //
    }

    public void StartWave()
    {

    }

    public void EndWave()
    {
        // Kill all enemies
        foreach (var enemy in spawnedEnemies)
            if(enemy)
                enemy.Despawn();
        spawnedEnemies.Clear();
    }
}
