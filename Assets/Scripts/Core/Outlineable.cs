
using System;
using EditorAttributes;
using LineworkLite;
using LineworkLite.Common.Attributes;
using LineworkLite.FreeOutline;
using UnityEngine;

public class Outlinable : MonoBehaviour
{
    [SerializeField] [RenderingLayerMask] private int outlineLayer = 3; 
    [SerializeField, Required] private FreeOutlineSettings outlineSettings;
    [SerializeField] private int outlineIndex = 0;
    [SerializeField] private Renderer manualRenderer;
    
    private Renderer[] renderers;
    private uint originalLayer;

    private void Start()  
    {
        if (!manualRenderer)
            renderers = TryGetComponent<MeshRenderer>(out var meshRenderer)
                ? new[] { meshRenderer }
                : GetComponentsInChildren<MeshRenderer>();
        else
            renderers = new[] { manualRenderer };
        originalLayer = renderers[0].renderingLayerMask;  
    }

    public void SetOutline(bool enable, Color outlineColor = default)
    {
        if (renderers == null)
            return;
        
        if (enable && outlineColor != default)
            outlineSettings.Outlines[outlineIndex].color = outlineColor;
        
        foreach (var rend in renderers)  
        {            
            if(!rend) 
                continue;
            
            rend.renderingLayerMask = enable 
                ? originalLayer | 1u << outlineLayer - 1
                : originalLayer;  
        }    
    }

    public void OverrideRenderer(Renderer newOverride)
    {
        renderers = new[] { newOverride };
    }
}