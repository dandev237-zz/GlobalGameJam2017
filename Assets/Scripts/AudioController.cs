using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    private float dbNeeded = 0F;
    private int lowFrequency = 0, highFrequency = 0;

    public AudioMixer mixer;
    public AudioSource source;

    private int sampleRate, samples;
    private float noise, pitch;
    private float[] data, spectrum;
    private List<GameObject> high, low;

    // Use this for initialization
    void Start()
    {
        samples = 1024;
        lowFrequency = 40;
        highFrequency = 400;
        dbNeeded = -10F;
        sampleRate = AudioSettings.outputSampleRate;
        data = new float[samples];
        spectrum = new float[samples];
        source.clip = Microphone.Start(Microphone.devices[0], true, 10, sampleRate);
        source.loop = true;
        while (Microphone.GetPosition(null) <= 0) ;
        source.Play();
        high = new List<GameObject>();
        low = new List<GameObject>();
    }

    void OnDestroy()
    {
        Microphone.End(null);
    }

    // Update is called once per frame
    void Update()
    {
        Analyze();
        if (noise >= dbNeeded && pitch >= lowFrequency && pitch < highFrequency)
        {
            PlayerController.Scream();
            foreach(GameObject go in low)
            {
                Destroy(go);
            }
            low.Clear();
        }
        else if (noise >= dbNeeded && pitch >= highFrequency)
        {
            PlayerController.Scream();
            foreach (GameObject go in high)
            {
                Destroy(go);
            }
            high.Clear();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("High")) high.Add(collision.gameObject);
        else if (collision.tag.Equals("Low")) low.Add(collision.gameObject);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("High")) high.Remove(collision.gameObject);
        else if (collision.tag.Equals("Low")) low.Remove(collision.gameObject);
    }

    /// <summary>
    /// Analyzes audio input and gets the noise percentage and the pitch
    /// </summary>
    private void Analyze()
    {
        // Get noise level
        source.GetOutputData(data, 0);
        float sum = 0;
        for (int i = 0; i < samples; i++)
        {
            sum += Mathf.Pow(data[i], 2);
        }
        noise = Mathf.Sqrt(sum / samples);
        noise = 20 * Mathf.Log10(noise / 0.1F);
        Debug.Log(noise);

        // Get pitch
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxWave = spectrum[0];
        int idx = 0;
        for (int i = 1; i < spectrum.Length; i++)
        {
            if (maxWave < spectrum[i])
            {
                maxWave = spectrum[i];
                idx = i; // Max frequency!
            }
        }
        pitch = idx * (sampleRate / 2) / samples;
    }
}
