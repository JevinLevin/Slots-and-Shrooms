using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void OnClickPlay()
    {
        GameManager.Instance.StartLoadTherapy();
    }
}
