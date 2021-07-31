using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    private float xRotation;
    private float yRotation;

    public float movementSensitivity = 12.0f;

    Vector3 move;

    void Start()
    {
        xRotation = Camera.main.transform.rotation.eulerAngles.x;
        yRotation = Camera.main.transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            //xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            yRotation += mouseX;

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSensitivity = 120.0f;
        }
        else
        {
            movementSensitivity = 12.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("UpDown");
        float z = Input.GetAxis("Vertical");

        move = transform.right * x + transform.up * y + transform.forward * z;
        transform.position += move * movementSensitivity * Time.deltaTime;



    }

}
