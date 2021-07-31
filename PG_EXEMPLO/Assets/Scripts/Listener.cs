using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Listener : MonoBehaviour
{
    [Header("Sphere Properties")]
    public float radius;
    public float x;
    public float y;
    public float z;

    [Header("Sound Properties")]
    public float energyFactor;
    public float[] soundLevel;
    public float[] pressure;
    
    int numFreq = 7;
    int numRays;
    public RayTracing rayTracing;
    public float dephtAtPosition;

    private void Start()
    {
        radius = GetComponent<SphereCollider>().radius;

        energyFactor = 4.0f / radius;
        soundLevel = new float[numFreq];
        pressure = new float[numFreq];

        numRays = 0;

        for (int i = 0; i < soundLevel.Length; i++)
        {
            soundLevel[i] = 0.0f;
            pressure[i] = 0.0f;
        }

    }

    /*private void Update()
    {
        energyFactor = 4.0f / (GetComponent<SphereCollider>().radius * rayTracing.readFile.cellsize);
        soundLevel = new float[numFreq];
        pressure = new float[numFreq];
        for (int i = 0; i < soundLevel.Length; i++)
        {
            soundLevel[i] = 0.0f;
            pressure[i] = 0.0f;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
        {
            dephtAtPosition = hit.distance * rayTracing.readFile.cellsize;
        }

        numRays = 0;
    }*/

    public string PrintAnswer()
    {
        //string fileName = "Results.csv";
        //StreamWriter sr = File.CreateText(fileName);
        string line = "";
        for (int i = 0; i < soundLevel.Length; i++)
        {
            //Debug.Log("Value " + i + ": " + (soundLevel[i]/ numRays));
            //Debug.Log(numRays);
            line = line + PressureToSoundLevel(pressure[i] / numRays).ToString() + " | ";
            //line = line + (soundLevel[i]).ToString() + " ";
            //Debug.Log("Value " + i + ": " + PressureToSoundLevel(pressure[i] / numRays));

            //Debug.Log("Value " + i + ": " + (10 * Mathf.Log10(soundLevel[i])));
            //Debug.Log("Value " + i + ": " + (soundLevel[i]));
        }
        //Debug.Log(this.gameObject.name + line);
        //sr.WriteLine(line);
        //sr.Close();
        return line;
    }

    public void SumEnergyListener(float[] level)
    {
        numRays += 1;
        for (int i = 0; i < level.Length; i++)
        {
            soundLevel[i] += energyFactor * level[i];

            //soundLevel[i] += level[i];
        }
    }

    public void SumPressureListener(float[] pressu)
    {
        numRays += 1;
        for (int i = 0; i < pressu.Length; i++)
        {
            pressure[i] += energyFactor * pressu[i];

            /*
            pressure[i] += Mathf.Pow(energyFactor, 2) * pressu[i];
            pressure[i] += pressu[i];
            soundLevel[i] += energyFactor * PressureToSoundLevel(pressu[i]);
            soundLevel[i] += energyFactor * Mathf.Pow(10, PressureToSoundLevel(pressu[i])/10);
            */
        }
    }

    float PressureToSoundLevel(float pressure)
    {
        return 20 * Mathf.Log10(pressure / Mathf.Pow(10, -6));
    }

    float[] AllPressureToSoundLevel(float[] pressures)
    {
        float[] levels = new float[pressures.Length];
        for (int i = 0; i < pressures.Length; i++)
        {
            levels[i] = PressureToSoundLevel(pressures[i]);
        }

        return levels;
    }

    public void SetRadius(float r)
    {
        radius = r;
    }

    public void RecalculateEnergyFactor()
    {
        energyFactor = 4.0f / radius;
    }

    public void ResetRaysNumber()
    {
        numRays = 0;
    }

    public void ResetLevelAndPressure()
    {
        for (int i = 0; i < soundLevel.Length; i++)
        {
            soundLevel[i] = 0.0f;
            pressure[i] = 0.0f;
        }
    }

}
