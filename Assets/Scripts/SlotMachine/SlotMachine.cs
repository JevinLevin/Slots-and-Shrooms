using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Unity.Cinemachine;

public class SlotMachine : MonoBehaviour, IInteractable
{
    
    [SerializeField] private SlotMachineDisc[] discs;
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private Renderer mainSlotMachineRenderer;
    [SerializeField] private Renderer handleRenderer;

    [Header("Spinning")] 
    [Tooltip("How many slots to pass per second")]
    [SerializeField] private int slotsPerSecond = 1;
    // This naming is sooo bad
    [Tooltip("Delay after you start spinning before stopping any slots")]
    [SerializeField] private Vector2 spinStartStopDelayRange = new Vector2(3,5);
    [Tooltip("Delay after a slot stops before the next one starts to stop")]
    [SerializeField] private Vector2 spinStartStopNextDelayRange = new Vector2(0.5f,1);
    [Tooltip("Random of slots to pass before a slot comes to a complete stop after they start stopping")]
    [SerializeField] private Vector2Int spinStopDelayRange = new Vector2Int(6,20);
    [SerializeField] private AnimationCurve spinStopCurve;
    [Tooltip("Multiplier for the stopping time, meant to account for the little animation on the spin which would cause the spinning to initially speed up.")]
    [SerializeField] private float stopTimeMultiplier = 1.5f;
    
    [SerializeField] private List<MushroomAttributeSO> attributes;
    [SerializeField] private List<MushroomRarityStats> rarityStats;

    private bool activated;
    private Vector2 rarityRange;
    private int maxWeight = 0;
    private List<SlotMachineDisc> spinningDiscs;

    public int SlotsPerSecond => slotsPerSecond;
    public float SpinAngleSpeed => (slotsPerSecond * 36);
    public int GetRandomStopDelay => Random.Range(spinStopDelayRange.x, spinStopDelayRange.y);
    public AnimationCurve GetStopCurve => spinStopCurve;
    public float StopTimeMultiplier => stopTimeMultiplier;

    private void Awake()
    {
        foreach (MushroomRarityStats rarityStat in rarityStats)
        {
            maxWeight += rarityStat.weight; 
        }
        Deactivate();
    }

    private void Start()
    {
        Activate();
    }

    private void Activate()
    {
        activated = true;
    }

    private void Deactivate()
    {
        activated = false;
        camera.enabled = false;
    }

    public void OnInteract(Interactor interactor)
    {
        if (!activated)
            return;

        StartSpinning();

        // interactor.GetComponent<MushroomInventory>().AddMushroom(GetRandomMushroomType());
    }

    private Mushroom GetRandomMushroomType()
    {
        MushroomRarityStats pickedRarity = new MushroomRarityStats();
        int rarityRoll = Random.Range(0, maxWeight);
        int runningTotal = 0;

        foreach (MushroomRarityStats rarityStat in rarityStats)
        {
            runningTotal += rarityStat.weight;
            if (runningTotal >= rarityRoll)
            {
                pickedRarity = rarityStat;
                break;
            }
        }

        List<MushroomAttributeSO> mushroomAttributes = new List<MushroomAttributeSO>();

        int points = pickedRarity.points;
        int maxAttributesWeight = 0;
        foreach (MushroomAttributeSO attribute in attributes)
        {
            maxAttributesWeight += attribute.Weight;
        }

        int dam = 0;
        bool pointsSpent = false;
        while (!pointsSpent)
        {
            int roll = Random.Range(0, maxAttributesWeight);
            int attributeRunningTotal = 0;
            foreach (MushroomAttributeSO attribute in attributes)
            {
                attributeRunningTotal += attribute.Weight;
                if (attributeRunningTotal >= roll)
                {
                    points -= attribute.SelectionCost;
                    if (points <= 0) pointsSpent = true;

                    attribute.OnSelected();
                    mushroomAttributes.Add(attribute);
                    break;
                }
            }
            dam++;
            if (dam > 10)
            {
                Debug.Log("DAMED");
                break;
            }
        }

        return new Mushroom(mushroomAttributes);
    }

    private void StartSpinning()
    {
        camera.enabled = true;
        camera.Priority = 1000;

        spinningDiscs = new();   
        foreach (var disc in discs)
        {
            disc.StartSpinning(this);
            spinningDiscs.Add(disc);
        }

        StartCoroutine(nameof(SpinDiscs));
    }

    private IEnumerator SpinDiscs()
    {
        float startStopTime = 0.0f;
        float startStopDelay = Random.Range(spinStartStopDelayRange.x, spinStartStopDelayRange.y);
        while (startStopTime < startStopDelay)
        {
            startStopTime += Time.deltaTime;
            yield return null;
        }

        while (spinningDiscs.Count > 0)
        {
            var currentDisc = spinningDiscs[0];
            
            currentDisc.StartStopSpinning();
            
            spinningDiscs.RemoveAt(0);
            if (spinningDiscs.Count > 0)
            {
                float nextDelay = Random.Range(spinStartStopNextDelayRange.x, spinStartStopNextDelayRange.y);
                yield return new WaitForSeconds(nextDelay);
            }
            else
            {
                while (currentDisc.IsStopping)
                    yield return null;
            }
        }

        Deactivate();

    }
}
 