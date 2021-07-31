using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class WaterLayerGenerator : MonoBehaviour
{
    //Map size
    float col;
    float row;

    //Mesh variables
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;



    //Layer text objects
    [Header("Layer Properties UI Objects")]
    public Text layerText;
    public InputField speedInputField;
    public InputField densityInputField;
    public InputField temperatureInputField;
    public InputField salinityInputField;
    public InputField phInputField;
    public InputField initialDepthInputField;
    public GameObject layerMenu;

    List<Text> listTextLayer = new List<Text>();
    List<InputField> listInputFieldsSpeed = new List<InputField>();
    List<InputField> listInputFieldsDensity = new List<InputField>();
    List<InputField> listInputFieldsTemperature = new List<InputField>();
    List<InputField> listInputFieldsSalinity = new List<InputField>();
    List<InputField> listInputFieldsPH = new List<InputField>();
    List<InputField> listInputFieldsInitialDepth = new List<InputField>();

    public GameObject waterLayerObject;

    public List<GameObject> layers = new List<GameObject>();

    public float temperatureAux = 5.0f;
    public float phAux = 8.0f;
    public float salinityAux = 38.0f;
    public float initialDepthAux = 0.0f;

    public float speedAux = 1550.0f;
    public float densityAux = 1050.0f;

    void RemoveAllLayers()
    {
        //Destroy Objects
        foreach (GameObject o in layers)
        {
            Destroy(o);
        }
        foreach (Text o in listTextLayer)
        {
            Destroy(o.gameObject);
        }
        foreach (InputField o in listInputFieldsSpeed)
        {
            Destroy(o.gameObject);
        }
        foreach (InputField o in listInputFieldsDensity)
        {
            Destroy(o.gameObject);
        }
        foreach (InputField o in listInputFieldsTemperature)
        {
            Destroy(o.gameObject);
        }
        foreach (InputField o in listInputFieldsSalinity)
        {
            Destroy(o.gameObject);
        }
        foreach (InputField o in listInputFieldsPH)
        {
            Destroy(o.gameObject);
        }
        foreach (InputField o in listInputFieldsInitialDepth)
        {
            Destroy(o.gameObject);
        }


        //Clear Lists
        layers = new List<GameObject>();
        listTextLayer = new List<Text>();
        listInputFieldsSpeed = new List<InputField>();
        listInputFieldsDensity = new List<InputField>();
        listInputFieldsTemperature = new List<InputField>();
        listInputFieldsSalinity = new List<InputField>();
        listInputFieldsPH = new List<InputField>();
        listInputFieldsInitialDepth = new List<InputField>();


    }

    public void SetXY(float a, float b)
    {
        col = a;
        row = b;

        CreateFirstLayer();

        if (layers.Count > 0)
        {
            RemoveAllLayers();
        }

        layers.Add(Instantiate(waterLayerObject));
        layers[0].GetComponent<WaterLayer>().row = row;
        layers[0].GetComponent<WaterLayer>().col = col;
        layers[0].GetComponent<WaterLayer>().CreateShape();

        layers[0].GetComponent<WaterLayer>().temperature = 8.0f;
        layers[0].GetComponent<WaterLayer>().ph = 8.0f;
        layers[0].GetComponent<WaterLayer>().salinity = 35.0f;
        layers[0].GetComponent<WaterLayer>().speed = 1500.0f;
        layers[0].GetComponent<WaterLayer>().density = 1035.0f;

        //Layer Text
        listTextLayer.Add(Instantiate(layerText, layerMenu.transform));
        listTextLayer[listTextLayer.Count - 1].text = listTextLayer.Count + ":";
        listTextLayer[listTextLayer.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listTextLayer.Count - 1), 0);

        //Speed InputField
        listInputFieldsSpeed.Add(Instantiate(speedInputField, layerMenu.transform));
        listInputFieldsSpeed[listInputFieldsSpeed.Count - 1].text = "1500";
        listInputFieldsSpeed[listInputFieldsSpeed.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsSpeed.Count - 1), 0);

        //Desnity InputField
        listInputFieldsDensity.Add(Instantiate(densityInputField, layerMenu.transform));
        listInputFieldsDensity[listInputFieldsDensity.Count - 1].text = "1035";
        listInputFieldsDensity[listInputFieldsDensity.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsDensity.Count - 1), 0);

        //Temperature InputField
        listInputFieldsTemperature.Add(Instantiate(temperatureInputField, layerMenu.transform));
        listInputFieldsTemperature[listInputFieldsTemperature.Count - 1].text = "8";
        listInputFieldsTemperature[listInputFieldsTemperature.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsTemperature.Count - 1), 0);

        //Salinity InputField
        listInputFieldsSalinity.Add(Instantiate(salinityInputField, layerMenu.transform));
        listInputFieldsSalinity[listInputFieldsSalinity.Count - 1].text = "35";
        listInputFieldsSalinity[listInputFieldsSalinity.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsSalinity.Count - 1), 0);

        //pH InputField
        listInputFieldsPH.Add(Instantiate(phInputField, layerMenu.transform));
        listInputFieldsPH[listInputFieldsPH.Count - 1].text = "8";
        listInputFieldsPH[listInputFieldsPH.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsPH.Count - 1), 0);

        //Initial Depth InputField
        listInputFieldsInitialDepth.Add(Instantiate(initialDepthInputField, layerMenu.transform));
        listInputFieldsInitialDepth[listInputFieldsInitialDepth.Count - 1].text = (10 * (listInputFieldsInitialDepth.Count - 1)).ToString();
        listInputFieldsInitialDepth[listInputFieldsInitialDepth.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsInitialDepth.Count - 1), 0);
        listInputFieldsInitialDepth[listInputFieldsInitialDepth.Count - 1].readOnly = true;
    }

    void CreateFirstLayer()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();

        vertices = new Vector3[4];

        vertices[0] = new Vector3(0.0f, 0.0f, 0.0f);
        vertices[1] = new Vector3(0.0f, 0.0f, col);
        vertices[2] = new Vector3(row, 0.0f, 0.0f);
        vertices[3] = new Vector3(row, 0.0f, col);

        triangles = new int[6];

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 1;
        triangles[4] = 3;
        triangles[5] = 2;

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    public void AddLayer()
    {
        layers.Add(Instantiate(waterLayerObject));
        layers[layers.Count - 1].GetComponent<WaterLayer>().row = row;
        layers[layers.Count - 1].GetComponent<WaterLayer>().col = col;
        layers[layers.Count - 1].GetComponent<WaterLayer>().initialDepth = layers[layers.Count - 2].GetComponent<WaterLayer>().initialDepth + 10;
        layers[layers.Count - 1].GetComponent<WaterLayer>().CreateShape();

        layers[layers.Count - 1].GetComponent<WaterLayer>().temperature = temperatureAux;
        layers[layers.Count - 1].GetComponent<WaterLayer>().ph = phAux;
        layers[layers.Count - 1].GetComponent<WaterLayer>().salinity = salinityAux;
        layers[layers.Count - 1].GetComponent<WaterLayer>().speed = speedAux;
        layers[layers.Count - 1].GetComponent<WaterLayer>().density = densityAux;

        layers[layers.Count - 2].GetComponent<WaterLayer>().finalDepth = layers[layers.Count - 1].GetComponent<WaterLayer>().initialDepth;
        layers[layers.Count - 2].GetComponent<WaterLayer>().CreateBottomShape();

        //Layer Text
        listTextLayer.Add(Instantiate(layerText, layerMenu.transform));
        listTextLayer[listTextLayer.Count - 1].text = listTextLayer.Count + ":";
        listTextLayer[listTextLayer.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listTextLayer.Count - 1), 0);

        //Speed InputField
        listInputFieldsSpeed.Add(Instantiate(speedInputField, layerMenu.transform));
        listInputFieldsSpeed[listInputFieldsSpeed.Count - 1].text = layers[layers.Count - 1].GetComponent<WaterLayer>().speed.ToString();
        listInputFieldsSpeed[listInputFieldsSpeed.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsSpeed.Count - 1), 0);

        //Desnity InputField
        listInputFieldsDensity.Add(Instantiate(densityInputField, layerMenu.transform));
        listInputFieldsDensity[listInputFieldsDensity.Count - 1].text = layers[layers.Count - 1].GetComponent<WaterLayer>().density.ToString();
        listInputFieldsDensity[listInputFieldsDensity.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsDensity.Count - 1), 0);

        //Temperature InputField
        listInputFieldsTemperature.Add(Instantiate(temperatureInputField, layerMenu.transform));
        listInputFieldsTemperature[listInputFieldsTemperature.Count - 1].text = layers[layers.Count - 1].GetComponent<WaterLayer>().temperature.ToString();
        listInputFieldsTemperature[listInputFieldsTemperature.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsTemperature.Count - 1), 0);

        //Salinity InputField
        listInputFieldsSalinity.Add(Instantiate(salinityInputField, layerMenu.transform));
        listInputFieldsSalinity[listInputFieldsSalinity.Count - 1].text = layers[layers.Count - 1].GetComponent<WaterLayer>().salinity.ToString();
        listInputFieldsSalinity[listInputFieldsSalinity.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsSalinity.Count - 1), 0);

        //pH InputField
        listInputFieldsPH.Add(Instantiate(phInputField, layerMenu.transform));
        listInputFieldsPH[listInputFieldsPH.Count - 1].text = layers[layers.Count - 1].GetComponent<WaterLayer>().ph.ToString();
        listInputFieldsPH[listInputFieldsPH.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsPH.Count - 1), 0);

        //Initial Depth InputField
        listInputFieldsInitialDepth.Add(Instantiate(initialDepthInputField, layerMenu.transform));
        listInputFieldsInitialDepth[listInputFieldsInitialDepth.Count - 1].text = layers[layers.Count - 1].GetComponent<WaterLayer>().initialDepth.ToString();
        listInputFieldsInitialDepth[listInputFieldsInitialDepth.Count - 1].transform.position -= new Vector3(0, Screen.height * 0.07f * (listInputFieldsInitialDepth.Count - 1), 0);

        


    }

    

    public void RemoveLayer()
    {
        if (listTextLayer.Count > 1)
        {
            Destroy(listTextLayer[listTextLayer.Count - 1].gameObject);
            Destroy(listInputFieldsSpeed[listInputFieldsSpeed.Count - 1].gameObject);
            Destroy(listInputFieldsDensity[listInputFieldsDensity.Count - 1].gameObject);
            Destroy(listInputFieldsTemperature[listInputFieldsTemperature.Count - 1].gameObject);
            Destroy(listInputFieldsSalinity[listInputFieldsSalinity.Count - 1].gameObject);
            Destroy(listInputFieldsPH[listInputFieldsPH.Count - 1].gameObject);
            Destroy(listInputFieldsInitialDepth[listInputFieldsInitialDepth.Count - 1].gameObject);

            listTextLayer.RemoveAt(listInputFieldsSpeed.Count - 1);
            listInputFieldsSpeed.RemoveAt(listInputFieldsSpeed.Count - 1);
            listInputFieldsDensity.RemoveAt(listInputFieldsDensity.Count - 1);
            listInputFieldsTemperature.RemoveAt(listInputFieldsTemperature.Count - 1);
            listInputFieldsSalinity.RemoveAt(listInputFieldsSalinity.Count - 1);
            listInputFieldsPH.RemoveAt(listInputFieldsPH.Count - 1);
            listInputFieldsInitialDepth.RemoveAt(listInputFieldsInitialDepth.Count - 1);


        }

    }

    public void ApplyChange()
    {
        for (int i = 1; i < layers.Count-1; i++)
        {
            layers[i].GetComponent<WaterLayer>().initialDepth = float.Parse(listInputFieldsInitialDepth[i].text);
            layers[i-1].GetComponent<WaterLayer>().finalDepth = float.Parse(listInputFieldsInitialDepth[i].text);
            layers[i-1].GetComponent<WaterLayer>().CreateBottomShape();
        }

        for (int i = 0; i < layers.Count; i++)
        {
            layers[i].GetComponent<WaterLayer>().speed = float.Parse(listInputFieldsSpeed[i].text);
            layers[i].GetComponent<WaterLayer>().density = float.Parse(listInputFieldsDensity[i].text);
            layers[i].GetComponent<WaterLayer>().temperature = float.Parse(listInputFieldsTemperature[i].text);
            layers[i].GetComponent<WaterLayer>().salinity = float.Parse(listInputFieldsSalinity[i].text);
            layers[i].GetComponent<WaterLayer>().ph = float.Parse(listInputFieldsPH[i].text);
            layers[i].GetComponent<WaterLayer>().initialDepth = float.Parse(listInputFieldsInitialDepth[i].text);
            layers[layers.Count - 1].GetComponent<WaterLayer>().CreateShape();

            if (i == layers.Count - 1)
            {
                layers[i].GetComponent<WaterLayer>().finalDepth = -1;
            }
            else
            {
                layers[i].GetComponent<WaterLayer>().finalDepth = layers[i+1].GetComponent<WaterLayer>().initialDepth;
                layers[i].GetComponent<WaterLayer>().CreateBottomShape();
            }
            
        }

        /*
        layers[layers.Count - 1].GetComponent<WaterLayer>().initialDepth = float.Parse(listInputFieldsInitialDepth[layers.Count - 1].text);
        layers[layers.Count - 2].GetComponent<WaterLayer>().finalDepth = float.Parse(listInputFieldsInitialDepth[layers.Count - 1].text);
        layers[layers.Count - 1].GetComponent<WaterLayer>().CreateShape();
        layers[layers.Count - 2].GetComponent<WaterLayer>().CreateBottomShape();
        */
        

    }
}
