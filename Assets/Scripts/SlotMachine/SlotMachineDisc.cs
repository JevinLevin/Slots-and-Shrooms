using System.Collections;
using UnityEngine;

public class SlotMachineDisc : MonoBehaviour
{
    public const int DISC_SLOT_COUNT = 10;
    public const float DISC_SLOT_ANGLE = 360 / DISC_SLOT_COUNT;
    
    [SerializeField] private SlotMachineSlot[] slots;
    public bool IsSpinning { get; private set; }
    public bool IsStopping { get; private set; }

    private SlotMachineStorage slotMachine;

    private int targetSlot;
    private float totalSpinAngle;
    private float currentSpinAngle;
    private float targetSpinAngle;
    private int slotsTillStop;
    
    public void StartSpinning(SlotMachineStorage slotMachine)
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
        int currentSlot = (int)(totalSpinAngle / DISC_SLOT_ANGLE);
        int slotIndex = currentSlot % DISC_SLOT_COUNT;
        
        // Update texture 2 slots ahead
        int updateIndex = (slotIndex + 2) % DISC_SLOT_COUNT;
        slots[updateIndex].NewTexture();
    }
}
