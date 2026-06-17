using System;
using System.Collections;
using UnityEngine;

public class SlotMachineDisc : MonoBehaviour
{
    public const int DISC_SLOT_COUNT = 10;
    public const float DISC_SLOT_ANGLE = 360 / DISC_SLOT_COUNT;
    
    [SerializeField] private SlotMachineSlot[] slots;
    [SerializeField] private MushroomGeneratorSO mushroomGenerator;
    [SerializeField] private Outlinable outline;
    [SerializeField] private Color invalidColor;
    [SerializeField] private Color validColor;

    public bool IsSpinning { get; private set; }
    public bool IsStopping { get; private set; }
    public Color GetOutlineColor => slotMachine.IsSpinning ? invalidColor : validColor;
    public Mushroom GetMushroomData => mushroomData;

    public int GetCurrentSlotIndex(float angleOffset = 0)
    {
        int currentSlot = (int)((totalSpinAngle + angleOffset) / DISC_SLOT_ANGLE);
        return currentSlot % DISC_SLOT_COUNT;
    }

    private SlotMachine slotMachine;
    private SlotMachineSlot winningSlot;
    private Mushroom mushroomData;

    private int targetSlot;
    private float totalSpinAngle;
    private float currentSpinAngle;
    private float targetSpinAngle;
    private int slotsTillStop;

    public void ResetDisc()
    {
        transform.rotation = Quaternion.AngleAxis(0, Vector3.right);
        targetSlot = 0;
        totalSpinAngle = 0;
        currentSpinAngle = 0;
        targetSpinAngle = 0;
        slotsTillStop = 0;
    }

    public void StartSpinning(SlotMachine slotMachine)
    {

        IsSpinning = true;
        IsStopping = false;
        this.slotMachine = slotMachine;
        
        // Update textures on initiali slots
        slots[0].NewTexture();
        slots[1].NewTexture();
        slots[2].NewTexture();
        slots[^1].NewTexture();

        StartCoroutine(nameof(Spinning));
    }
    
    private IEnumerator Spinning()
    {
        while (!IsStopping)
        {
            float frameSpinAngle = slotMachine.SpinAngleSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, frameSpinAngle);
            UpdateSlots(frameSpinAngle);

            yield return null;
        }


        float maxStopTime = ((float)slotsTillStop / slotMachine.SlotsPerSecond) * slotMachine.StopTimeMultiplier;
        float stopTime = 0;
        float startSpinAngle = totalSpinAngle;
        
        while (stopTime <= maxStopTime)
        {
            float t = stopTime / maxStopTime;
            float stopProgress = slotMachine.GetStopCurve.Evaluate(t);

            float currentAngle = Mathf.LerpUnclamped(startSpinAngle, targetSpinAngle, 1-stopProgress);

            transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.right);

            float angleOffset = currentAngle - totalSpinAngle;
            
            UpdateSlots(angleOffset);
            
            stopTime += Time.deltaTime;

            yield return null;
        }
        
        StopSpinning();
    }

    public void StartStopSpinning()
    {
        
        IsStopping = true;
        
        // Stop at a random slot
        slotsTillStop += slotMachine.GetRandomStopDelay;

        float remainingSpinAngle = slotsTillStop * DISC_SLOT_ANGLE;
        // Account for angle between end of current slot
        remainingSpinAngle += (DISC_SLOT_ANGLE - currentSpinAngle);

        targetSpinAngle = totalSpinAngle + remainingSpinAngle;
    }

    public void StopSpinning()
    {
        IsSpinning = false;
        IsStopping = false;

        winningSlot = slots[GetCurrentSlotIndex(DISC_SLOT_ANGLE / 2)];
        mushroomData = mushroomGenerator.GetMushroom();
        mushroomData.SetTexture(winningSlot.CurrentTexture);
    }

    private void UpdateSlots(float angle)
    {
        
        totalSpinAngle += angle;
        currentSpinAngle += angle;
        

        // Every 36 degrees spun, a new slot appears
        int slotsPassed = (int)(currentSpinAngle / DISC_SLOT_ANGLE);
        if (slotsPassed >= 1)
        {
            currentSpinAngle %= DISC_SLOT_ANGLE;
            for(int i = 0; i < slotsPassed; i++)
                OnSlotPassed();
        }
        //print("update slot " + totalSpinAngle);

    }

    private void OnSlotPassed()
    {
        int slotIndex = GetCurrentSlotIndex();

        outline.OverrideRenderer(slots[slotIndex].GetRenderer);


        // Update texture 2 slots ahead
        int updateIndex = (slotIndex + 2) % DISC_SLOT_COUNT;

        slots[updateIndex].NewTexture();

    }

    private void OnMouseOver()
    {
        OnHover();
    }

    private void OnMouseExit()
    {
        OnUnhover();
    }



    private void OnHover()
    {
        if (!slotMachine || !slotMachine.Activated || IsSpinning || slotMachine.IsReplacing || !slotMachine.IsChoosing)
            return;
        
        outline.SetOutline(true, GetOutlineColor);

        // Dont rehover
        if (slotMachine.CurrentHoveredDisc == this)
            return;

        slotMachine.OnHoverDisc(this);
    }


    private void OnUnhover()
    {
        if (!slotMachine || slotMachine.CurrentHoveredDisc != this || slotMachine.IsReplacing || !slotMachine.IsChoosing)
            return;

        Unhover();

    }

    private void Unhover()
    {
        outline.SetOutline(false);

        slotMachine.OnUnhoverDisc(this);
    }

    private void OnMouseDown()
    {
        OnSelect();
    }

    private void OnSelect()
    {
        if (!slotMachine || slotMachine.CurrentHoveredDisc != this || slotMachine.IsReplacing)
            return;

        slotMachine.OnSelectDisc(this);

        Unhover();
    }
}
