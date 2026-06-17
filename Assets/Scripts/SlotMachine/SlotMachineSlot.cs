using System;
using UnityEngine;

public class SlotMachineSlot : MonoBehaviour
{
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    
    [SerializeField] private MeshRenderer mushroomRenderer;

    public Texture2D CurrentTexture { get; private set; }
    public Renderer GetRenderer => mushroomRenderer;


    private void Awake()
    {
        mushroomRenderer.material = new Material(mushroomRenderer.material);
    }

    public void NewTexture()
     {
        CurrentTexture = MushroomStudio.Instance.TakeSnapshot();
         mushroomRenderer.material.SetTexture(BaseMap, CurrentTexture);
     }

}
