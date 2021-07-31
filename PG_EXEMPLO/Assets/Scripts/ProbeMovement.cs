using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeMovement : MonoBehaviour
{
    public GameObject source;
    public GameObject listener;
    public int probetype = 0; //0 = source; 1 = listener
    public int movementtype = 0; //0 = move; 1 = rotate

    Vector3 originS;
    Vector3 originL;

    void Start()
    {
        originS = source.transform.position;
        originL = listener.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            probetype = 0;
        }
        if (Input.GetKey(KeyCode.C))
        {
            probetype = 1;
        }

        if (Input.GetKey(KeyCode.N))
        {
            movementtype = 0;
        }
        if (Input.GetKey(KeyCode.M))
        {
            movementtype = 1;
        }

        if (probetype == 0)
        {
            if (movementtype == 0)
            {
                Movement(source);
            }
            else
            {
                Rotation(source);
            }
            
        }
        else
        {
            Movement(listener);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            source.transform.position = originS;
            source.transform.rotation = Quaternion.identity;
            listener.transform.position = originL;
            listener.transform.rotation = Quaternion.identity;
        }


    }

    void Movement(GameObject obj)
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            obj.transform.position += new Vector3(-0.1f,0,0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            obj.transform.position += new Vector3(0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            obj.transform.position += new Vector3(0, 0.1f, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            obj.transform.position += new Vector3(0, -0.1f, 0);
        }
    }

    void Rotation(GameObject obj)
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            obj.transform.eulerAngles += new Vector3(-0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            obj.transform.eulerAngles += new Vector3(0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            obj.transform.eulerAngles += new Vector3(0, 0, 0.1f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            obj.transform.eulerAngles += new Vector3(0, 0, -0.1f);
        }
    }
}
