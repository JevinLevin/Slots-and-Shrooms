using UnityEngine;

[CreateAssetMenu(fileName = "TowageAttribute", menuName = "AttributeSOs/TowageAttribute")]
public class TowageOnHit : OnHitMushroomAttributeSO
{
    [Header("Towage Settings")]
    [SerializeField] private float strength; 
    public override void OnHit(GameObject objHit, GameObject attacker)
    {
        if (objHit.CompareTag("Player")) return;
        Debug.Log("ONHIT");
        Vector3 fowards = (objHit.transform.position - attacker.transform.position).normalized;
        fowards.y = 0f;

        objHit.transform.position -= fowards * strength;
    }

    public override void OnSelected()
    {
        Debug.Log("Tow"); 
    }

    public override void OnTick()
    {
        throw new System.NotImplementedException();
    }
}
