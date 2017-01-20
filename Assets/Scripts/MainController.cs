using UnityEngine;
using UnityEngine.SceneManagement;


public class MainController : MonoBehaviour
{
    private GameObject panelHow, panelMain;

    public void Awake()
    {
        panelMain = GameObject.Find("Canvas/PanelMain");
        panelHow = GameObject.Find("Canvas/PanelHowToPlay");
        panelHow.SetActive(false);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowHowToPlay()
    {
        panelMain.SetActive(false);
        panelHow.SetActive(true);
    }

    public void BackToMain()
    {
        panelHow.SetActive(false);
        panelMain.SetActive(true);
    }

}
