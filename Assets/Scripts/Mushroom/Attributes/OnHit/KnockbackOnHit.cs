using UnityEngine;

[CreateAssetMenu(fileName = "KnockbackAttribute", menuName = "AttributeSOs/KnockbackAttribute")]
public class KnockbackOnHit : OnHitMushroomAttributeSO
{
    [Header("Knockback Settings")]
    [SerializeField] private float strength = 2; 

    public override void OnHit(GameObject objHit, GameObject hitter)
    {
        if (objHit.CompareTag("Player")) return;
        Debug.Log("ONHIT");
        Vector3 fowards = objHit.transform.forward;
        fowards.y = 0f;

        objHit.transform.position -= fowards * strength;
    }

    public override void OnSelected()
    {
        Debug.Log("added knockback"); 
    }

    public override void OnTick()
    {
        throw new System.NotImplementedException();
    }
}
