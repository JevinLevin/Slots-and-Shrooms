using UnityEngine;
using System.Collections;

public class TherapyRoom : MonoBehaviour
{
    [SerializeField] private SlotMachine slotMachine;
    [SerializeField] private TherapyChair therapyChair;
    

    private IEnumerator Start()
    {
        therapyChair.ToggleState(false);

        yield return null;

        slotMachine.Activate(StartChair);   
    }
    private void StartChair()
    {
        therapyChair.ToggleState(true);
    }
}
