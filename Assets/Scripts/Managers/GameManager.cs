using UnityEngine;
using PrimeTween;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CanvasFader transitionCanvas;
    [SerializeField] private string gameSceneName;
    
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

    private void Initialise()
    {
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        PrimeTweenConfig.warnTweenOnDisabledTarget = false;
        PrimeTweenConfig.warnZeroDuration = false;
        PrimeTweenConfig.SetTweensCapacity(400);
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
        Cursor.lockState = cursorState ? CursorLockMode.None : CursorLockMode.Confined;
    }
    
    private void ForceSetCursor(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Confined;
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
