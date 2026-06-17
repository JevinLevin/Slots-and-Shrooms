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

    [SerializeField] private MushroomInfoUI mushroomInfoUI;
    [SerializeField] private SlotMachineDisc[] discs;
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private Renderer mainSlotMachineRenderer;
    [SerializeField] private Renderer handleRenderer;
    [SerializeField] private RenderingLayerMask outlineLayer;

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
    
    public bool Activated { get; private set; }
    private Vector2 rarityRange;
    private int maxWeight = 0;
    private List<SlotMachineDisc> spinningDiscs;
    private uint originalLayer;
    public SlotMachineDisc CurrentHoveredDisc { get; private set; }

    public int SlotsPerSecond => slotsPerSecond;
    public float SpinAngleSpeed => (slotsPerSecond * 36);
    public int GetRandomStopDelay => Random.Range(spinStopDelayRange.x, spinStopDelayRange.y);
    public AnimationCurve GetStopCurve => spinStopCurve;
    public float StopTimeMultiplier => stopTimeMultiplier;

    public static Action OnSlotMachineStartSpinning;
    public static Action OnSlotMachineStopSpinning;
    public static Action OnSlotMachineActivate;
    public static Action OnSlotMachineDeactivate;

    private void Awake()
    {
        originalLayer = mainSlotMachineRenderer.renderingLayerMask;
        Deactivate();
    }

    private void Start()
    {
        Activate();
    }

    private void Activate()
    {
        OnSlotMachineActivate?.Invoke();
        Activated = true;
        ToggleOutline(true);
    }

    private void Deactivate()
    {
        OnSlotMachineDeactivate?.Invoke();
        Activated = false;
        camera.enabled = false;
        GameManager.Instance.ToggleCursor(false);
    }

    private void ToggleOutline(bool value)
    {
        mainSlotMachineRenderer.renderingLayerMask = value
            ? originalLayer | 1u << outlineLayer - 1
            : originalLayer;
        handleRenderer.renderingLayerMask = value
            ? originalLayer | 1u << outlineLayer - 1
            : originalLayer;
    }

    public void OnInteract(Interactor interactor)
    {
        if (!Activated)
            return;

        StartSpinning();

        // interactor.GetComponent<MushroomInventory>().AddMushroom(GetRandomMushroomType());
    }

    private void StartSpinning()
    {
        OnSlotMachineStartSpinning?.Invoke();

        camera.enabled = true;
        camera.Priority = 1000;
        ToggleOutline(false);
        GameManager.Instance.ToggleCursor(true);

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

        StopSpinning();
    }

    private void StopSpinning()
    {
        OnSlotMachineStopSpinning?.Invoke();

        //Deactivate();
    }

    public void OnHoverDisc(SlotMachineDisc disc)
    {
        CurrentHoveredDisc = disc;
        mushroomInfoUI.ShowMushroomInfo(CurrentHoveredDisc.GetMushroomData);
    }
    public void OnUnhoverDisc(SlotMachineDisc disc)
    {
        if (CurrentHoveredDisc != disc)
            return;
        CurrentHoveredDisc = null;
        mushroomInfoUI.HideMushroomInfo();
    }
}
 