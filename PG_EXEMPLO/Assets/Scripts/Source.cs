using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : MonoBehaviour
{
    public float[] levels;
    public float totalLevel;
    public float[] pressures;
    RayTracing rayTracing;

    private void Start()
    {
        rayTracing = GetComponent<RayTracing>();

        //levels = new float[rayTracing.numFreq];
        pressures = new float[rayTracing.numFreq];

        InitializePressuresAndTotalLevel();
    }

    void InitializePressuresAndTotalLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            pressures[i] = LevelSoundToPressure(levels[i]);
        }

        totalLevel = TotalLevelSound();
    }

    float LevelSoundToPressure(float dB)
    {
        return Mathf.Pow(10, -6) * (Mathf.Pow(10, dB / 20));
    }

    float TotalLevelSound()
    {
        float sum = 0;
        for (int i = 0; i < levels.Length; i++)
        {
            sum += Mathf.Pow(10, levels[i] / 10);
        }

        return 10 * Mathf.Log10(sum);
    }
}
