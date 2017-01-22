using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using NAudio.Wave;
using System;
using UnityEngine.SceneManagement;

public struct DirsFilesLists
{
    public List<string> files, dirs;
}

public class MusicLoader : MonoBehaviour
{

    public event ErrorEvent MusicError;
    public delegate void ErrorEvent(string error);

    public IWavePlayer player;
    public float Length
    {
        get
        {
            return stream != null ? (float) stream.TotalTime.TotalSeconds : 0;
        }
    }

    private WaveStream stream;
    private WaveChannel32 channel;
    private MemoryStream mms;

    void Awake()
    {
        SceneManager.sceneLoaded += LevelLoaded;
    }

    public void OnDestroy()
    {
        if (player != null)
        {
            player.Stop();
            player.Dispose();
        }
    }

    public void Stop()
    {
        if (player.PlaybackState == PlaybackState.Playing) player.Pause();
        channel.Seek(0, SeekOrigin.Begin);
    }

    private void LevelLoaded(Scene game, LoadSceneMode mode)
    {
        if (game.name.Equals("Game"))
        {
            Stop();
            player.Play();
        }
    }

    public DirsFilesLists GetDirsAndFiles(string currentPath)
    {
        if (currentPath == null) currentPath = Directory.GetCurrentDirectory();
        DirsFilesLists dfl = new DirsFilesLists();
        dfl.files = new List<string>(Directory.GetFiles(currentPath, "*.mp3"));
        dfl.dirs = new List<string>(Directory.GetDirectories(currentPath));
        return dfl;
    }

    public void PlayTrack(string path)
    {
        StartCoroutine(LoadTrack(path));
    }

    private IEnumerator LoadTrack(string path)
    {
        if (!path.StartsWith("file:///")) path = "file:///" + path;
        WWW trackFile = new WWW(path);
        while (!trackFile.isDone) yield return trackFile;
        if (!string.IsNullOrEmpty(trackFile.error))
        {
            ErrorEvent handler = MusicError;
            if (handler != null)
            {
                handler(trackFile.error);
                yield break;
            }
        }

        string error = null;
        byte[] data = trackFile.bytes;
        if (player != null) player.Dispose();
        try
        {
            mms = new MemoryStream(data);
            stream = new Mp3FileReader(mms);
            channel = new WaveChannel32(stream);
            player = new WaveOutEvent();
            player.Init(channel);
            player.Play();
        }
        catch (Exception e)
        {
            error = e.ToString();
        }
        {
            ErrorEvent handler = MusicError;
            if (handler != null)
            {
                handler(error);
            }
        }
    }
}
