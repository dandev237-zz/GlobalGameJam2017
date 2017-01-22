using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public float noisePercentageNeeded = 0F;
    public int lowFrequency = 0, highFrequency = 0;

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
        if (lowFrequency == 0) lowFrequency = 40;
        if (highFrequency == 0) highFrequency = 400;
        if (noisePercentageNeeded < 0F || noisePercentageNeeded > 100F) noisePercentageNeeded = 30F;
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
        if (noise >= noisePercentageNeeded && pitch >= lowFrequency && pitch < highFrequency)
        {
            PlayerController.Scream();
            foreach(GameObject go in low)
            {
                Destroy(go);
            }
            low.Clear();
        }
        else if (noise >= noisePercentageNeeded && pitch >= highFrequency)
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
        noise = Mathf.Sqrt(sum / samples) * 100;

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
