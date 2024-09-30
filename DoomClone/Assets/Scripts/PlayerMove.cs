using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // movement vars
    public float playerSpeed = 20f;
    private float myGravity = -25f;

    // dash vars
    public float dashSpeed = 31f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;
    private bool isDashing = false;
    public float dashTime;

    private int dashCount = 0; // Track the number of consecutive dashes
    private float[] lastDashTimes = new float[2]; // Track cooldown for each dash
    
    // jump vars
    public float jumpHeight = 2f;
    private bool isJumping = false;
    private float verticalVelocity;

    // animation vars
    private CharacterController myCC;
    public Animator camAnim;
    private bool isWalking;

    // mouse vars
    private Vector3 inputVector;
    private Vector3 movementVector;

    // camera reference
    public Transform playerCamera;

    void Start()
    {
        myCC = GetComponent<CharacterController>();
        lastDashTimes[0] = -dashCooldown; // Initialize so that the first dash is ready immediately
        lastDashTimes[1] = -dashCooldown; // Initialize so that the second dash is ready immediately
    }

    void Update()
    {
        GetInput();
        MovePlayer();
        CheckForHeadBob();

        camAnim.SetBool("isWalking", isWalking);
    }

    void CheckForHeadBob()
    {
        if (myCC.velocity.magnitude > 0.1f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    void GetInput()
    {
        // Get input for movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Normalize the input so diagonal movement isn't faster
        inputVector = new Vector3(horizontal, 0f, vertical).normalized;

        // Get the camera's forward and right directions (ignoring the y component)
        Vector3 camForward = Vector3.Scale(playerCamera.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = playerCamera.right;

        // Calculate the movement vector relative to the camera's direction
        movementVector = (camForward * inputVector.z + camRight * inputVector.x) * playerSpeed;

        if (myCC.isGrounded)
        {
            verticalVelocity = -2f; // Small value to keep the player grounded
            isJumping = false;

            // Handle jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * myGravity);
                isJumping = true;
            }
        }
        else
        {
            verticalVelocity += myGravity * Time.deltaTime; // Apply gravity when in the air
        }

        movementVector.y = verticalVelocity; // Add vertical velocity (jumping/gravity)
    }

    void MovePlayer()
    {
        // Handle dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time > lastDashTimes[0] + dashCooldown)
            {
                StartDash(0); // Perform first dash
            }
            else if (Time.time > lastDashTimes[1] + dashCooldown)
            {
                StartDash(1); // Perform second dash
            }
        }

        if (isDashing)
        {
            Dash();
        }

        myCC.Move(movementVector * Time.deltaTime);
    }

    void StartDash(int dashIndex)
    {
        isDashing = true;
        dashTime = Time.time;
        lastDashTimes[dashIndex] = Time.time; // Set cooldown time for this dash
        dashCount++;
    }

    void Dash()
    {
        if (Time.time < dashTime + dashDuration)
        {
            myCC.Move(movementVector * Time.deltaTime * dashSpeed);
        }
        else
        {
            isDashing = false;
        }
    }
}
