using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    [SerializeField] Text text;

    public void SetMessage(string s)
    {
        text.text = s;
    }

    public void OkButton()
    {
        Destroy(this.gameObject);
    }
}
