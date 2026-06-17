using UnityEngine;

public class MushroomBase : MonoBehaviour
{
    [SerializeField] private Transform topPoint;
    public Vector3 TopPosition => topPoint.position;
}
