using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileMenu : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] Toggle normalResolutionToggle;
    [SerializeField] GameObject resolutionFactorsGameObject;

    public void ChangeNormalResolutionToggle()
    {
        if (normalResolutionToggle.isOn)
        {
            resolutionFactorsGameObject.gameObject.SetActive(false);
        }
        else
        {
            resolutionFactorsGameObject.gameObject.SetActive(true);
        }
    }

}
