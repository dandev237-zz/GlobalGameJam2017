using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MusicLoader : MonoBehaviour {

    public AudioSource audioSource;
    private List<AudioClip> musicTracks;

	void Start () {
        string[] fileNames = Directory.GetFiles("Music/");

        musicTracks = new List<AudioClip>();
        foreach(string file in fileNames){
            AudioClip clip = Resources.Load<AudioClip>("Music/" + file);
            musicTracks.Add(clip);
        }
	}
	
	public void PlayTrack()
    {
        AudioClip track = musicTracks[Random.Range(0, musicTracks.Count + 1)];
        audioSource.PlayOneShot(track);
    }
}
