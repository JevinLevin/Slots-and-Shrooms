using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryMushroomUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Image mushroomIcon;
    [SerializeField] private Outline outline;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text replaceText;

    [SerializeField] private InventoryUI inventoryUI;
    private Mushroom mushroomData;
    public Mushroom GetMushroomData => mushroomData;

    private bool hovering;

    private void Awake()
    {
        button.enabled = false;
        outline.enabled = false;
        replaceText.enabled = false;
    }

    public void Initialise(InventoryUI inventoryUI, Mushroom mushroomData)
    {
        this.inventoryUI = inventoryUI;
        this.mushroomData = mushroomData;

        Refresh();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;

        outline.enabled = true;
        replaceText.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!hovering)
            return;

        hovering = false;

        outline.enabled = false;
        replaceText.enabled = false;
    }

    public void Refresh()
    {
        mushroomIcon.sprite = MushroomStudio.ConvertToSprite(mushroomData.MushroomTexture);
    }


    public void StartReplacing()
    {
        button.enabled = true;

    }
    public void StopReplacing()
    {
        button.enabled = false;
    }

    public void OnClickReplace()
    {
        inventoryUI.OnClickReplace(mushroomData);
    }

    public void OnClickAdd()
    {
        print("try add");
        inventoryUI.OnClickAdd();
    }
}
