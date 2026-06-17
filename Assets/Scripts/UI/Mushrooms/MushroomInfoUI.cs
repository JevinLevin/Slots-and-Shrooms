using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MushroomInfoUI : MonoBehaviour
{
    [SerializeField] private CanvasFader canvasFader;
    [SerializeField] private MushroomBuffTextUI buffTextPrefab;
    [SerializeField] private GameObject buffTextRoot;
    [SerializeField] private MushroomBuffTextUI debuffTextPrefab;
    [SerializeField] private GameObject debuffTextRoot;
    [SerializeField] private Image mushroomImage;

    private List<MushroomBuffTextUI> activeTexts = new();

    private void Awake()
    {
        // Delete any buffs/debuffs left over
        foreach (Transform child in buffTextRoot.transform)
            Destroy(child.gameObject);
        foreach (Transform child in debuffTextRoot.transform)
            Destroy(child.gameObject);
    }

    public void ShowMushroomInfo(Mushroom mushroomData)
    {
        canvasFader.PlayIn();

        foreach (var buffText in activeTexts)
            Destroy(buffText.gameObject);
        activeTexts.Clear();

        foreach (var attribute in mushroomData.Attributes)
        {
            if (attribute.IsBuff)
                InstantiateText(buffTextPrefab, buffTextRoot, attribute.GetDescription);
            else
                InstantiateText(debuffTextPrefab, debuffTextRoot, attribute.GetDescription);


            mushroomImage.sprite = MushroomStudio.ConvertToSprite(mushroomData.MushroomTexture);
        }

    }

    private void InstantiateText(MushroomBuffTextUI prefab, GameObject root, string desc)
    {
        var newText = Instantiate(prefab, root.transform);
        newText.SetBuffText(desc);
        activeTexts.Add(newText);
    }

    public void HideMushroomInfo()
    {
        canvasFader.PlayOut();
    }
}
