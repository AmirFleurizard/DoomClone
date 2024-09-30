using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 1.4f;
    public float smoothing = 1.5f;

    private float xMousePos;
    public float yMousePos;
    private float smoothedXMousePos;
    private float smoothedYMousePos;

    private float currentLookingXPos;
    private float currentLookingYPos;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        getInput();
        ModifyInput();
        MovePlayer();
    }

    void getInput()
    {
        xMousePos = Input.GetAxisRaw("Mouse X");
        yMousePos = Input.GetAxisRaw("Mouse Y");
    }

    void ModifyInput()
    {
        xMousePos *= sensitivity * smoothing;
        yMousePos *= sensitivity * smoothing;
        smoothedXMousePos = Mathf.Lerp(smoothedXMousePos, xMousePos, 1f / smoothing);
        smoothedYMousePos = Mathf.Lerp(smoothedYMousePos, yMousePos, 1f / smoothing);
    }

    void MovePlayer()
    {
        currentLookingXPos += smoothedXMousePos;
        currentLookingYPos -= smoothedYMousePos;
        
        transform.localRotation = Quaternion.Euler(currentLookingYPos, currentLookingXPos, 0f);
    }
}
