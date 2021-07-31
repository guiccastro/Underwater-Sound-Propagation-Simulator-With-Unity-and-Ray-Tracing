using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] GameObject menu;
    [SerializeField] GameObject layer;
    [SerializeField] GameObject file;
    [SerializeField] GameObject rayTracing;
    [SerializeField] GameObject source;
    [SerializeField] GameObject listener;
    [SerializeField] Toggle csvFile;
    [SerializeField] Toggle ascFile;


    //Menu
    public void Start()
    {
        menu.SetActive(true);

    }


    //Layer
    public void OpenLayer()
    {
        menu.SetActive(false);
        layer.SetActive(true);
    }

    public void CloseLayer()
    {
        layer.SetActive(false);
        menu.SetActive(true);
    }

    //File
    public void OpenFile()
    {
        menu.SetActive(false);
        file.SetActive(true);
    }

    public void CloseFile()
    {
        file.SetActive(false);
        menu.SetActive(true);
    }

    public void OpenRayTracing()
    {
        menu.SetActive(false);
        rayTracing.SetActive(true);
    }

    public void CloseRayTracing()
    {
        rayTracing.SetActive(false);
        menu.SetActive(true);
    }

    public void OpenSource()
    {
        menu.SetActive(false);
        source.SetActive(true);
    }

    public void CloseSource()
    {
        source.SetActive(false);
        menu.SetActive(true);
    }

    public void OpenListener()
    {
        menu.SetActive(false);
        listener.SetActive(true);
    }

    public void CloseListener()
    {
        listener.SetActive(false);
        menu.SetActive(true);
    }

    public void CheckCSVFile()
    {
        ascFile.isOn = false;
    }

    public void CheckASCFile()
    {
        csvFile.isOn = false;
    }

    public void OpenExplorer()
    {
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe");
        p.Start();
    }



}
