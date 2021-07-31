using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SourceMenu : MonoBehaviour
{
    [Header("Source GameObject")]
    [SerializeField] GameObject source;

    [Header("ReadFile pointer")]
    [SerializeField] ReadFile readFile;

    [Header("Text gameobject")]
    [SerializeField] Text currentDepthText;

    [Header("Variables")]
    float xPosition;
    float yPosition;
    float zPosition;
    float radius;

    float[] Levels = new float[7]; 

    [Header("Source InputFields")]
    [SerializeField] InputField xInputField;
    [SerializeField] InputField yInputField;
    [SerializeField] InputField zInputField;
    [SerializeField] InputField radiusInputField;

    [Header("Source Level InputFields")]
    [SerializeField] InputField _31_5InputField;
    [SerializeField] InputField _63InputField;
    [SerializeField] InputField _125InputField;
    [SerializeField] InputField _250InputField;
    [SerializeField] InputField _500InputField;
    [SerializeField] InputField _1000InputField;
    [SerializeField] InputField _2000InputField;

    [Header("Variables constants")]
    static float defaultPosition = 0.0f;
    static float defaultRadius = 0.5f;

    [Header("Mask for raycast")]
    public LayerMask layerMask;

    public void ChangeValueXYZ()
    {
        if (xInputField.text != "")
        {
            if (xInputField.text != "-")
            {
                xPosition = float.Parse(xInputField.text.Replace('.', ',')) * readFile.sizeFactor;
            }
        }
        else
        {
            xPosition = defaultPosition;
        }

        if (yInputField.text != "")
        {
            if (yInputField.text != "-")
            {
                yPosition = float.Parse(yInputField.text.Replace('.', ',')) * readFile.depthFactor;
            }
        }
        else
        {
            yPosition = defaultPosition;
        }

        if (zInputField.text != "")
        {
            if (yInputField.text != "-")
            {
                zPosition = float.Parse(zInputField.text.Replace('.', ',')) * readFile.sizeFactor;
            }
        }
        else
        {
            zPosition = defaultPosition;
        }
        source.transform.localPosition = new Vector3(xPosition, yPosition, zPosition);

        ChangeCurrentDepthText();
    }
    
    void ChangeCurrentDepthText()
    {
        RaycastHit hit;
        if (Physics.Raycast(source.transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            currentDepthText.text = "Depth at position: " + (hit.distance / readFile.depthFactor);
        }
    }

    public void ChangeRadius()
    {
        if (radiusInputField.text != "")
        {
            radius = float.Parse(radiusInputField.text.Replace('.', ','));
        }
        else
        {
            radius = defaultRadius;
        }

        source.GetComponent<SphereCollider>().radius = radius;

        float radius2 = 2 * radius;
        source.transform.localScale = new Vector3(radius2, radius2, radius2);
    }

    public void ChangeLevels()
    {
        Levels[0] = float.Parse(_31_5InputField.text.Replace('.', ','));
        Levels[1] = float.Parse(_63InputField.text.Replace('.', ','));
        Levels[2] = float.Parse(_125InputField.text.Replace('.', ','));
        Levels[3] = float.Parse(_250InputField.text.Replace('.', ','));
        Levels[4] = float.Parse(_500InputField.text.Replace('.', ','));
        Levels[5] = float.Parse(_1000InputField.text.Replace('.', ','));
        Levels[6] = float.Parse(_2000InputField.text.Replace('.', ','));

        source.GetComponent<Source>().levels = Levels;
    }
}
