using UnityEngine;

[CreateAssetMenu(fileName = "TakeDmgOverTimePassive", menuName = "AttributeSOs/TakeDmgOverTimePassive")]
public class TakeDmgOverTimeDebuff : PassiveMushroomAttributeSO
{
    GameObject player;
    [SerializeField] float dmg;
    public override void OnSelected()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void PassiveAbillity()
    {
        Debug.Log("DAMAGEAGAG");
        player.GetComponent<IHasHealth>().OnHit(dmg, player); 
    }
}
