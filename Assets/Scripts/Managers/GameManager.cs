using UnityEngine;
using PrimeTween;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CanvasFader transitionCanvas;
    [SerializeField] private string therapySceneName;
    [SerializeField] private string gameSceneName;
    [SerializeField] private PlayerStatsHolderSO playerStats;
    [SerializeField] private MushroomGeneratorSO mushroomGenerator;
    [SerializeField] private MushroomInventorySO mushroomInventory;
    [SerializeField] private int maxFrameRate = 120;

    #region Singleton
    public static GameManager Instance { get; private set; }
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

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Application.targetFrameRate = maxFrameRate;
    }

    private void Initialise()
    {
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        PrimeTweenConfig.warnTweenOnDisabledTarget = false;
        PrimeTweenConfig.warnZeroDuration = false;
        PrimeTweenConfig.SetTweensCapacity(400);

        playerStats.Initialise();
        mushroomGenerator.Initialise();
        mushroomInventory.Initialise();
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

        mushroomInventory.Deinitialise();
    }

    private bool cursorDisabled;
    private bool cursorState = true;
    public void ForceDisableCursor(bool value)
    {
        cursorDisabled = value;
        if(value)
            ForceSetCursor(false);
        else
            SetCursor();
            
    }

    public void ToggleCursor(bool value)
    {
        cursorState = value;
        
        if(!cursorDisabled)
            SetCursor();
    }

    private void SetCursor()
    {
        Cursor.visible = cursorState;
        Cursor.lockState = cursorState ? CursorLockMode.None : CursorLockMode.Locked;
    }
    
    private void ForceSetCursor(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void StartLoadTherapy()
    {
        transitionCanvas.OnFadeInEnd += LoadTherapy;
        transitionCanvas.PlayIn();
    }
    private void LoadTherapy()
    {
        transitionCanvas.OnFadeInEnd -= LoadTherapy;
        SceneManager.LoadScene(therapySceneName);
        transitionCanvas.PlayOut();
    }
    public void StartLoadGame()
    {
        transitionCanvas.OnFadeInEnd += LoadGame;
        transitionCanvas.PlayIn();
    }

    private void LoadGame()
    {
        transitionCanvas.OnFadeInEnd -= LoadGame;
        SceneManager.LoadScene(gameSceneName);
        transitionCanvas.PlayOut();
    }
}
