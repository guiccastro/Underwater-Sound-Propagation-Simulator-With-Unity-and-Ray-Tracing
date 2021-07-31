using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ListenersManager : MonoBehaviour
{
    public List<GameObject> listeners = new List<GameObject>();

    [Header("Listener Menu Prefabs")]
    [SerializeField] Dropdown listenersDropdown;
    [SerializeField] Text listenerIndexText;
    [SerializeField] InputField xInputField;
    [SerializeField] InputField yInputField;
    [SerializeField] InputField zInputField;
    [SerializeField] Text radiusText;
    [SerializeField] InputField radiusInputField;

    [Header("Object Sphere Prefab")]
    [SerializeField] GameObject listenerPrefab;

    [Header("ReadFile pointer")]
    [SerializeField] ReadFile readFile;

    [Header("Text gameobject")]
    [SerializeField] Text currentDepthText;


    [Header("Listener properties variables")]
    float xPosition;
    float yPosition;
    float zPosition;
    float radius;

    [Header("Variables constants")]
    static float defaultPosition = 0.0f;
    static float defaultRadius = 0.5f;

    [Header("Mask for raycast")]
    public LayerMask layerMask;


    public void AddListener()
    {

        if (listeners.Count == 0)
        {
            listenerIndexText.gameObject.SetActive(true);
            xInputField.gameObject.SetActive(true);
            yInputField.gameObject.SetActive(true);
            zInputField.gameObject.SetActive(true);
            radiusText.gameObject.SetActive(true);
            radiusInputField.gameObject.SetActive(true);
        }

        listeners.Add(Instantiate(listenerPrefab, transform.position, Quaternion.identity, transform));

        listenersDropdown.options.Clear();

        for (int i = 0; i < listeners.Count; i++)
        {
            listenersDropdown.options.Add(new Dropdown.OptionData() { text = "Listener " + i.ToString() });
        }
    }

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

        listeners[listenersDropdown.value].transform.localPosition = new Vector3(xPosition, yPosition, zPosition);

        ChangeCurrentDepthText();
    }

    void ChangeCurrentDepthText()
    {
        RaycastHit hit;
        if (Physics.Raycast(listeners[listenersDropdown.value].transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
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

        listeners[listenersDropdown.value].GetComponent<SphereCollider>().radius = radius;

        float radius2 = 2 * radius;
        listeners[listenersDropdown.value].transform.localScale = new Vector3(radius2, radius2, radius2);

        listeners[listenersDropdown.value].GetComponent<Listener>().SetRadius(radius);
        listeners[listenersDropdown.value].GetComponent<Listener>().RecalculateEnergyFactor();

    }

    public void RemoveListener()
    {
        if (listeners.Count > 0)
        {
            Destroy(listeners[listenersDropdown.value]);
            listeners.RemoveAt(listenersDropdown.value);
            listenersDropdown.value = listenersDropdown.value - 1;

            listenersDropdown.options.Clear();

            for (int i = 0; i < listeners.Count; i++)
            {
                listenersDropdown.options.Add(new Dropdown.OptionData() { text = "Listener " + i.ToString() });
            }
        }
        

    }

    public void SelectNewListener()
    {
        listenerIndexText.text = "Listener " + listenersDropdown.value.ToString();

        xInputField.text = listeners[listenersDropdown.value].transform.position.x.ToString();
        yInputField.text = listeners[listenersDropdown.value].transform.position.y.ToString();
        zInputField.text = listeners[listenersDropdown.value].transform.position.z.ToString();

        radiusInputField.text = listeners[listenersDropdown.value].GetComponent<SphereCollider>().radius.ToString();
    }
}
