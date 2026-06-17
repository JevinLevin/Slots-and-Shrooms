using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Random = UnityEngine.Random;

public class MushroomStudio : MonoBehaviour
{
    [SerializeField] private RenderTexture mushroomTexture;
    [SerializeField] private Camera mushroomCamera;
    [SerializeField] private MushroomBase[] bases;
    [SerializeField] private MushroomTop[] tops;

    #region Singleton
    public static MushroomStudio Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        Initialise();
    }
    #endregion
    
    private void Initialise()
    {
        DisableAllMushrooms();
    }
    public Texture2D TakeSnapshot()
    {
        MushroomBase randomBase = bases[Random.Range(0, bases.Length - 1)];
        MushroomTop randomTop = tops[Random.Range(0, tops.Length - 1)];
        
        randomBase.gameObject.SetActive(true);
        randomTop.gameObject.SetActive(true);

        randomTop.transform.position = randomBase.TopPosition;

        mushroomCamera.Render();
        
        var output = ToTexture2D(mushroomTexture);
        DisableAllMushrooms();
        return output;
    }
    
    Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBAFloat, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    private void DisableAllMushrooms()
    {
        foreach(var mushroomBase in bases)
            mushroomBase.gameObject.SetActive(false);
        foreach(var mushroomTop in tops)
            mushroomTop.gameObject.SetActive(false);
    }


    // [Button]
    // private void Debug_GenerateMushroomTexture()
    // {
    //     DisableAllMushrooms();
    //     testMesh.material = new Material(testMesh.material);
    //     testMesh.material.SetTexture(BaseMap, TakeSnapshot());
    // }
}
