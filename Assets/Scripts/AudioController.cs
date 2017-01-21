using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public event FrequencyEvent OnLowFrequency;
    public event FrequencyEvent OnHighFrequency;

    public delegate void FrequencyEvent();

    public float noisePercentageNeeded = 0F;
    public int samples = 0, lowFrequency = 0, highFrequency = 0;

    public AudioMixer mixer;
    public AudioSource source;

    private int sampleRate;
    private float noise, pitch;
    private float[] data, spectrum;

    // Use this for initialization
    void Start () {
        if (samples == 0) samples = 1024;
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
    }

    void OnDestroy()
    {
        Microphone.End(null);
    }

    // Update is called once per frame
    void Update () {
        Analyze();
        if (noise >= noisePercentageNeeded && pitch >= lowFrequency && pitch < highFrequency)
        {
            FrequencyEvent handler = OnLowFrequency;
            if (handler != null) handler();
        }
        else if (noise >= noisePercentageNeeded && pitch >= highFrequency)
        {
            FrequencyEvent handler = OnHighFrequency;
            if (handler != null) handler();
        }
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
