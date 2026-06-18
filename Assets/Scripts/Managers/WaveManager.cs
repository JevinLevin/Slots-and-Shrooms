using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using PrimeTween;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private SlotMachine slotMachine;
    [SerializeField] GameObject player;
    [SerializeField] private GameOverUI gameOver;

    [Header("Time Attributes")] 
    [SerializeField] private int waveDuration = 60;
    
    [Header("Spawning Attributes")] 
    [SerializeField] private Vector2Int enemySpawnCountRange = new(10, 200);
    [SerializeField] private AnimationCurve enemySpawnCountCurve;
    [Tooltip("Enemies dont stop spawning after this wave, the spawn count will just keep scaling after this point")] 
    [SerializeField] private int enemySpawnMaxWave = 20;

    [Header("Ending")]
    [SerializeField] private float gameOverDelay = 2;
    
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
    
    public bool WaveActive {get; private set;}

    public static Action<int> OnWaveTimeChanged;
    public static Action<int> OnNewWave;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDie += GameEnd;
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerDie -= GameEnd;
    }

    void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        CurrentWave = 1;
        
        StartCoroutine(nameof(PlayWave));
    }

    public void GameEnd()
    {
        StopCoroutine(nameof(PlayWave));

        Tween.Delay(gameOverDelay, () => gameOver.Show(CurrentWave));
    }

    private IEnumerator PlayWave()
    {
        yield return null;

        WaveStart();

        while (WaveTime > 0)
        {
            yield return new WaitForSeconds(1);
            WaveTime--;
        }

        WaveEnd();
    }

    private void WaveStart()
    {
        WaveTime = waveDuration;
        
        EnemyManager.Instance.StartWave(GetWaveEnemyCount());

        OnNewWave?.Invoke(CurrentWave);
    }

    private void WaveEnd()
    {
        EnemyManager.Instance.EndWave();

        SpawnSlotMachine();
    }

    private void WaveNext()
    {
        CurrentWave++;

        StartCoroutine(nameof(PlayWave));
    }

    private void SpawnSlotMachine()
    {
        // Position infront of player
        Vector3 targetPos = player.transform.position + (player.transform.forward * 1.5f);
        // Get position on nav mesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas))
        {
            targetPos = hit.position;
        }

        Vector3 targetDirection = (player.transform.position - targetPos).normalized;

        slotMachine.Activate(targetPos, targetDirection, WaveNext);
    }

    private int GetWaveEnemyCount()
    {
        float t = (float)CurrentWave / enemySpawnMaxWave;
        float adjustedT = enemySpawnCountCurve.Evaluate(t);
        return (int)Mathf.Lerp(enemySpawnCountRange.x, enemySpawnCountRange.y, adjustedT);
    }
}
