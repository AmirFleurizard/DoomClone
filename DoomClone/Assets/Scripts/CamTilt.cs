using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTilt : MonoBehaviour
{
    public float tiltAmountZ = 4f;
    public float tiltAmountX = 2f;
    public float smoothSpeed = 7f;

    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial rotation of the camera to apply tilting correctly
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Get movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate the target tilt based on movement input
        float targetTiltX = moveZ * tiltAmountX;
        float targetTiltZ = moveX * -tiltAmountZ;

        // Get the current rotation (from mouse look) and add the tilt
        Quaternion targetRotation = initialRotation * Quaternion.Euler(targetTiltX, 0f, targetTiltZ);

        // Smoothly interpolate to the new tilted rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}
