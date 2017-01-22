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

    public event MusicEvent MusicLoaded;
    public delegate void MusicEvent(string error, AudioClip clip);

    public IWavePlayer player = new WaveOutEvent();

    private WaveStream stream;
    private WaveChannel32 channel;
    private AudioClip clip;
    private MemoryStream mms;

    void Awake()
    {
        SceneManager.sceneLoaded += LevelLoaded;
    }

    public void OnDestroy()
    {
        player.Stop();
        player.Dispose();
        channel.Dispose();
        stream.Dispose();
    }

    private void LevelLoaded(Scene game, LoadSceneMode mode)
    {
        if (game.name.Equals("Game"))
        {
            if (player.PlaybackState == PlaybackState.Playing) player.Pause();
            channel.Seek(0, SeekOrigin.Begin);
            MusicEvent handler = MusicLoaded;
            if (handler != null)
            {
                handler(null, clip);
            }
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
            MusicEvent handler = MusicLoaded;
            if (handler != null)
            {
                handler(trackFile.error, null);
                yield break;
            }
        }
        clip = trackFile.audioClip;

        string error = null;
        byte[] data = trackFile.bytes;
        try
        {
            mms = new MemoryStream(data);
            stream = new Mp3FileReader(mms);
            channel = new WaveChannel32(stream);
            player.Init(channel);
            player.Play();
        }
        catch (Exception e)
        {
            error = e.ToString();
        }
        {
            MusicEvent handler = MusicLoaded;
            if (handler != null)
            {
                handler(error, trackFile.audioClip);
            }
        }
    }
}
