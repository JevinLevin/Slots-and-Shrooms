using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Time Attributes")] 
    [SerializeField] private int waveDuration = 60;
    
    [Header("Spawning Attributes")] 
    [SerializeField] private Vector2Int enemySpawnCountRange = new(10, 200);
    [SerializeField] private AnimationCurve enemySpawnCountCurve;
    [Tooltip("Enemies dont stop spawning after this wave, the spawn count will just keep scaling after this point")] 
    [SerializeField] private int enemySpawnMaxWave = 20;
    
    public int CurrentWave { get; private set; }

    private int waveTime;
    public int WaveTime
    {
        get => waveTime;
        private set
        {
            waveTime = value;
            OnWaveTimeChanged?.Invoke(WaveTime);
        }
    }

    public static Action<int> OnWaveTimeChanged;
    void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        CurrentWave = 0;
        WaveTime = waveDuration;
        
        StartCoroutine(nameof(PlayWave));
    }

    private IEnumerator PlayWave()
    {
        while (WaveTime > 0)
        {
            yield return new WaitForSeconds(1);
            WaveTime--;
        }
    }

    private int GetWaveEnemyCount()
    {
        float t = (float)CurrentWave / enemySpawnMaxWave;
        float adjustedT = enemySpawnCountCurve.Evaluate(t);
        return (int)Mathf.Lerp(enemySpawnCountRange.x, enemySpawnCountRange.y, adjustedT);
    }
}
