using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private MushroomInfoUI mushroomInfoUI;
    [SerializeField] private Popup popup;
    [SerializeField] private MushroomInventorySO mushroomInventory;
    [SerializeField] private InventoryMushroomUI inventoryMushroomPrefab;
    [SerializeField] private Transform inventoryMushroomRoot;
    [SerializeField] private InventoryMushroomUI addUI;

    private List<InventoryMushroomUI> inventoryMushrooms = new();

    private bool replacing;
    private Mushroom replacingMushroom;
    private Action onReplaceEndCallback;

    private bool PressingInventoryButton => Input.GetKeyDown(KeyCode.I);
    private bool PressingEscButton => Input.GetKeyDown(KeyCode.Escape);

    #region Singleton
    public static InventoryUI Instance { get; private set; }
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

    }
    #endregion

    private void Start()
    {
        Initialise();
    }

    private void Update()
    {
        if(!popup.IsDisplaying && PressingInventoryButton)
            popup.Display();
        else if(popup.IsDisplaying && (PressingInventoryButton || PressingEscButton) && !replacing)
            popup.Hide();
    }

    private void OnEnable()
    {
        mushroomInventory.OnMushroomAdded += AddMushroomUI;
        mushroomInventory.OnMushroomRemoved += RemoveMushroomUI;
    }

    private void OnDisable()
    {
        mushroomInventory.OnMushroomAdded -= AddMushroomUI;
        mushroomInventory.OnMushroomRemoved -= RemoveMushroomUI;
    }

    private void RemoveMushroomUI(Mushroom mushroomData)
    {
        // Find inventory mushroom with matching data
        foreach(var inventoryMushroom in inventoryMushrooms)
        {
            if (inventoryMushroom.GetMushroomData != mushroomData)
                continue;

            Destroy(inventoryMushroom.gameObject);
            inventoryMushrooms.Remove(inventoryMushroom);

            break;
        }
    }

    private void AddMushroomUI(Mushroom mushroomData)
    {
        inventoryMushrooms.Add(CreateInventoryMushroom(mushroomData));
    }

    public void OnClickAdd()
    {
        mushroomInventory.AddMushroom(replacingMushroom);
        EndReplace();
    }

    private void Initialise()
    {
        foreach (var mushroomData in mushroomInventory.GetMushrooms)
        {
            inventoryMushrooms.Add(CreateInventoryMushroom(mushroomData));
        }
        addUI.gameObject.SetActive(false);
    }

    public void Refresh()
    {
        foreach(var inventoryMushroom in inventoryMushrooms)
        {
            inventoryMushroom.Refresh();
        }
    }

    private InventoryMushroomUI CreateInventoryMushroom(Mushroom mushroomData)
    {
        var newMushroom = Instantiate(inventoryMushroomPrefab, inventoryMushroomRoot);
        newMushroom.Initialise(this, mushroomData);
        return newMushroom;
    }

    public void TryReplace(Mushroom replacingMushroom, Action callback = null)
    {
        // If no mushrooms yet then nothing to replace
        if (inventoryMushrooms.Count == 0)
        {
            AddMushroomUI(replacingMushroom);
            callback?.Invoke();
            return;
        }

        replacing = true;
        this.replacingMushroom = replacingMushroom;
        onReplaceEndCallback = callback;

        foreach (var inventoryMushroom in inventoryMushrooms)
            inventoryMushroom.StartReplacing();

        addUI.gameObject.SetActive(true);
        addUI.StartReplacing();
        
        popup.Display();
    }

    public void EndReplace()
    {
        replacing = false;
        foreach (var inventoryMushroom in inventoryMushrooms)
            inventoryMushroom.StopReplacing();

        popup.Hide();

        onReplaceEndCallback?.Invoke();
        addUI.gameObject.SetActive(false);
        addUI.StopReplacing();
    }

    public void OnClickReplace(Mushroom mushroomData)
    {
        mushroomInventory.RemoveMushroom(mushroomData);
        mushroomInventory.AddMushroom(replacingMushroom);
        EndReplace();
    }

    public void ShowInfo(Mushroom mushroomData)
    {
        mushroomInfoUI.ShowMushroomInfo(mushroomData);
    }

    public void HideInfo()
    {
        mushroomInfoUI.HideMushroomInfo();
    }
}
