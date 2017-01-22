using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public GameObject songButton;

    private GameObject panelHow, panelMain, panelSong, music, content;
    private Text errorMsg;
    private Button playButton, upButton;
    private MusicLoader loader;
    private string currentPath, song = null;

    public void Awake()
    {
        music = GameObject.Find("MusicLoader");
        if (music == null)
        {
            music = new GameObject("MusicLoader");
            DontDestroyOnLoad(music);
            loader = music.AddComponent<MusicLoader>();
        }
        else loader = music.GetComponent<MusicLoader>();

        loader.MusicError += OnMusicError;

        panelMain = GameObject.Find("Canvas/PanelMain");
        panelHow = GameObject.Find("Canvas/PanelHowToPlay");
        panelHow.SetActive(false);
        content = GameObject.Find("Canvas/SongChooser/View/Viewport/Content");
        playButton = GameObject.Find("Canvas/SongChooser/Control/PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(() =>
        {
            if (song == null)
            {
                errorMsg.text = "Select a song first!";
                errorMsg.color = Color.cyan;
                return;
            }
            loader.Stop();
            panelSong.SetActive(false);
            panelHow.SetActive(true);
            Invoke("LoadSceneGame", 5);
        });
        upButton = GameObject.Find("Canvas/SongChooser/Control/ParentFolderButton").GetComponent<Button>();
        upButton.onClick.AddListener(() =>
        {
            DirectoryInfo tmp = Directory.GetParent(currentPath);
            if (tmp != null)
            {
                CleanUI();
                currentPath = tmp.FullName;
                NavigatePath();
            }
            else if (SystemInfo.operatingSystem.Contains("Windows"))
            {
                CleanUI();
                currentPath = "";
                foreach (string drive in Directory.GetLogicalDrives())
                {
                    GameObject button = Instantiate(songButton);
                    button.transform.SetParent(content.transform, false);
                    button.transform.localScale = new Vector3(1, 1, 1);
                    Button cmpButton = button.GetComponent<Button>();
                    Text textB = button.GetComponentInChildren<Text>();
                    string d = drive;
                    textB.text = d;
                    cmpButton.onClick.AddListener(() =>
                    {
                        currentPath = d;
                        CleanUI();
                        NavigatePath();
                    });
                }
            }
        });
        errorMsg = GameObject.Find("Canvas/SongChooser/Control/ErrorText").GetComponent<Text>();
        panelSong = GameObject.Find("Canvas/SongChooser");
        panelSong.SetActive(false);
    }

    private void OnDestroy()
    {
        loader.MusicError -= OnMusicError;
    }

    public void OnMusicError(string error)
    {
        if (error != null)
        {
            errorMsg.color = Color.red;
            errorMsg.text = error;
            song = null;
        }
    }

    public void LoadGame()
    {
        panelMain.SetActive(false);
        panelSong.SetActive(true);
        currentPath = Directory.GetCurrentDirectory();
        NavigatePath();
    }

    private void NavigatePath()
    {
        errorMsg.text = "You are at " + currentPath;
        errorMsg.color = Color.cyan;
        DirsFilesLists children = loader.GetDirsAndFiles(currentPath);
        foreach (string dir in children.dirs)
        {
            GameObject button = Instantiate(songButton);
            button.transform.SetParent(content.transform, false);
            button.transform.localScale = new Vector3(1, 1, 1);
            Button cmpButton = button.GetComponent<Button>();
            Text textB = button.GetComponentInChildren<Text>();
            string tmp = dir;
            textB.text = tmp;
            cmpButton.onClick.AddListener(() =>
            {
                song = null;
                currentPath = tmp;
                CleanUI();
                NavigatePath();
            });
        }
        foreach (string file in children.files)
        {
            GameObject button = Instantiate(songButton);
            button.transform.SetParent(content.transform, false);
            button.transform.localScale = new Vector3(1, 1, 1);
            Button cmpButton = button.GetComponent<Button>();
            Text textB = button.GetComponentInChildren<Text>();
            textB.color = Color.green;
            string tmp = file;
            textB.text = tmp;
            cmpButton.onClick.AddListener(() =>
            {
                song = file;
                loader.PlayTrack(file);
            });
        }
    }
    
    private void CleanUI()
    {
        foreach (Transform t in content.transform)
        {
            Destroy(t.gameObject);
        }
    }

    private void LoadSceneGame()
    {
        SceneManager.LoadScene("Game");
    }

}
