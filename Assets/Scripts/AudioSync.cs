using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSync : MonoBehaviour
{
    AudioSource mainAudioSource;
    int sampleRate;
    float maxFreq;
    float freqPerBand;
    [SerializeField, Range(64, 8192)] int nbFreqBand = 512;
    [SerializeField, Range(0, 0.1f)] float smoothingSpeed = 0.005f;
    [SerializeField, Range(1, 2)] float smoothingStrength = 1.2f;
    [SerializeField] List<int> freqLimits = new List<int> { 100, 300, 600, 1200, 2400, 4800, 10000, 1000000 };


    float[] equalizedSpectrum;
    public float[] EqualizedSpectrum { get { return equalizedSpectrum; } }


    private float[] spectrum;
    public float[] Spectrum { get { return spectrum; } }


    private float[] bufferSpectrum; 
    private float[] smoothRatio;

    [SerializeField] private float[] displayableSpectrum;
    public float[] DisplayableSpectrum { get { return displayableSpectrum; } }

    float[] maxSpectrumValues;

    void Awake()
    {
        mainAudioSource = GetComponent<AudioSource>();

        sampleRate = AudioSettings.outputSampleRate;
        maxFreq = sampleRate / 2;
        freqPerBand = maxFreq / nbFreqBand;

        spectrum = new float[nbFreqBand];
        equalizedSpectrum = new float[freqLimits.Count];
        displayableSpectrum = new float[equalizedSpectrum.Length];
        maxSpectrumValues = new float[displayableSpectrum.Length];
        bufferSpectrum = new float[displayableSpectrum.Length];
        smoothRatio = new float[displayableSpectrum.Length];
    }

    void FixedUpdate()
    {
        UpdateDisplayableSpectrum();
    }

    void UpdateSpectrum()
    {
        mainAudioSource.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
    }

    void UpdateEqualizedSpectrum()
    {
        equalizedSpectrum[0] = 0f;
        for (int i = 0, j = 0; i < spectrum.Length; i++)
        {
            float currentFreq = (i + 1) * freqPerBand;
            if(currentFreq > freqLimits[j])
            {
                j++;
                equalizedSpectrum[j] = 0f;
            }

            if (j < equalizedSpectrum.Length)
            {
                equalizedSpectrum[j] += spectrum[i];
            }
            else
            {
                Debug.LogError("Frequencies larger than the largest frequencies Interval");
            }
        }
    }

    float[] SmoothEqualizer(float[] spectrum)
    {
        for (int i = 0; i < spectrum.Length; i++)
        {

            if(spectrum[i] > bufferSpectrum[i])
            {

                bufferSpectrum[i] = spectrum[i];
                smoothRatio[i] = smoothingSpeed;
            }
            else
            {
                bufferSpectrum[i] -= smoothRatio[i];
                smoothRatio[i] *= smoothingStrength;
                if (bufferSpectrum[i] < 0)
                {
                    bufferSpectrum[i] = 0;
                }


            }
            spectrum[i] = bufferSpectrum[i];
        }
        return spectrum;
    }

    void UpdateDisplayableSpectrum()
    {
        UpdateSpectrum();
        UpdateEqualizedSpectrum();

        for (int i = 0; i < displayableSpectrum.Length; i++)
        {
            if(maxSpectrumValues[i] < equalizedSpectrum[i])
            {
                maxSpectrumValues[i] = equalizedSpectrum[i];
            }

            displayableSpectrum[i] = equalizedSpectrum[i] / maxSpectrumValues[i];
        }

        displayableSpectrum = SmoothEqualizer(displayableSpectrum);
    }


}
