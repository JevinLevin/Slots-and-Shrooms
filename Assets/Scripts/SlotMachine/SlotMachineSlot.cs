using System;
using UnityEngine;

public class SlotMachineSlot : MonoBehaviour
{
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    
    [SerializeField] private MeshRenderer mushroomRenderer;


    private void Awake()
    {
        mushroomRenderer.material = new Material(mushroomRenderer.material);
    }

    public void NewTexture()
     {
         mushroomRenderer.material.SetTexture(BaseMap, MushroomStudio.Instance.TakeSnapshot());
     }

}
