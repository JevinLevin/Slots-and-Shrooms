using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Popup popup;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private string waveTextFormat = "You made it to wave {0}";
    [SerializeField] private Button restartButton;


    public void Show(int wave)
    {
        popup.Display();
        waveText.text = string.Format(waveTextFormat, wave);
        restartButton.interactable = true;
    }

    public void OnClickRestart()
    {
        GameManager.Instance.Restart();

        restartButton.interactable = false;
    }


}
