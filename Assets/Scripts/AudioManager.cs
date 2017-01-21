using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioClip scream;

    // Use this for initialization
    void Awake()
    {
        //Load audioClips
    }

    public static AudioClip GetScream()
    {
        return scream;
    }
}
