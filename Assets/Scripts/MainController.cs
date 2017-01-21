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
        panelMain.SetActive(false);
        panelHow.SetActive(true);
        Invoke("LoadSceneGame", 5);
    }

    private void LoadSceneGame()
    {
        SceneManager.LoadScene("Game");
    }

}
