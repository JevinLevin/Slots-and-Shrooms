using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChainLightningAttribute", menuName = "AttributeSOs/ChainLightningAttribute")]
public class ChainLightningOnHit : OnHitMushroomAttributeSO
{
    [Header("Chain Lightning Settings")]
    [SerializeField] private float radius; 
    [SerializeField] private float damage;
    [SerializeField, Range(0,100)] private int chance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject visuals;
    [SerializeField] private AudioClip sound; 

    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    public override void OnHit(GameObject objHit, GameObject attacker)
    {
        if (!objHit.CompareTag("Enemy")) return;

        hitEnemies.Add(objHit.gameObject);

        int roll = Random.Range(0, 101);
        if (roll > chance) return;
        Debug.Log("CHAIN");

        bool hitEnemy = false;
        Collider[] colliders = Physics.OverlapSphere(objHit.transform.position, radius, layerMask);
        foreach (Collider collider in colliders)
        {
            if(collider.gameObject == objHit || hitEnemies.Contains(collider.gameObject)) continue;

            hitEnemy = true;
            collider.gameObject.GetComponent<IHasHealth>().OnHit(damage, attacker);

            GameObject vfx = Instantiate(visuals);
            LineRenderer lineRenderer = vfx.GetComponent<LineRenderer>();

            Vector3 objHitHeadPos = new Vector3(objHit.transform.position.x, objHit.transform.position.y + 1, objHit.transform.position.z);
            Vector3 colliderHeadPos = new Vector3(collider.transform.position.x, collider.transform.position.y + 1, collider.transform.position.z);

            lineRenderer.SetPosition(0, objHitHeadPos);
            lineRenderer.SetPosition(1, colliderHeadPos);

            AudioSource lightningAudio = vfx.GetComponent<AudioSource>();
            lightningAudio.Play();

            Destroy(vfx, lightningAudio.clip.length);

            break;
        }
        if(!hitEnemy) hitEnemies.Clear();
    }

    public override void OnSelected()
    {
        Debug.Log("ChainLightning");
    }

    public override void OnTick()
    {
        throw new System.NotImplementedException();
    }
}
