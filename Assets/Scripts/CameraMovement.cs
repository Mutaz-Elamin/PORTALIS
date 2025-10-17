using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Camera cam;
    private float xRotate = 0f;

    public float xSensitivity = 100f;
    public float ySensitivity = 100f;
    public float sprintOffset = 2.2f;
    public float sprintSmooth = 5f;
    public bool isSprinting = false;

    public float normalFOV = 60f;
    public float sprintFOV = 70f;
    public float fovSmooth = 5f;
    private Vector3 originalPos;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        xRotate = 0f;
        cam.transform.rotation = Quaternion.identity;
        originalPos = cam.transform.localPosition;
    }

    public void Look(Vector2 input)
    {
        float mouseX = input.x * xSensitivity * Time.deltaTime;
        float mouseY = input.y * ySensitivity * Time.deltaTime;

        xRotate -= mouseY;
        xRotate = Mathf.Clamp(xRotate, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotate, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        Vector3 targetPos = originalPos;
        if (isSprinting)
        {
            targetPos += new Vector3(0f, 0f, sprintOffset);
        }

        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, targetPos, Time.deltaTime * sprintSmooth);

        float targetFOV = isSprinting ? sprintFOV : normalFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * fovSmooth);

    }

}
