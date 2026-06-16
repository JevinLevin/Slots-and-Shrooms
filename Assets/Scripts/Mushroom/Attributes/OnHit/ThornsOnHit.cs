using UnityEngine;

[CreateAssetMenu(fileName = "ThornsAttribute", menuName = "AttributeSOs/ThornsAttribute")]
public class ThornsOnHit : OnHitMushroomAttributeSO
{
    [Header("Thorns Settings")]
    [SerializeField] private int damage; 

    public override void OnHit(GameObject objHit, GameObject attacker)
    {
        if (!objHit.CompareTag("Player")) return;
        IHasHealth health = attacker.GetComponent<IHasHealth>();
        health.OnHit(damage, attacker); 
    }

    public override void OnSelected()
    {
        
    }

    public override void OnTick()
    {
        throw new System.NotImplementedException();
    }
}
