using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;

    private GameObject playerHandle;
    private GameObject cameraHandle;

    private float tempEulerX;

    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
    }

    void Update()
    {
        playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.deltaTime);
        //cameraHandle.transform.Rotate(Vector3.right, pi.Jup * verticalSpeed * Time.deltaTime);
        tempEulerX -= pi.Jup * -verticalSpeed * Time.deltaTime;
        tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
        cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);
    }
}