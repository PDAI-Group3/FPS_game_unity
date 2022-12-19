using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 8f;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;

    private bool crouching;
    private float crouchTimer;
    private bool lerpCrouch;

    private bool sprinting;

    // hover
    public float DistanceFromGround = 2f;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;

            if (crouching)
            {
                controller.height = Mathf.Lerp(controller.height, 1, p);
                speed = 3;
            }
            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
                speed = 5;
            }

            if (p > 1) 
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

        /*
        //hover
        Vector3 newPos = transform.position;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            newPos.y = (hit.point + Vector3.up * DistanceFromGround).y;
        }

        transform.position = newPos;
        */
            

        
    }
    // this receives inputs for InputManager and applies to character controller

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        
        controller.Move(playerVelocity * Time.deltaTime);
        
    }
    public void Jump()
    {
        
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        /* //hover
        if (DistanceFromGround < 3)
        {
            playerVelocity.y = 5f;
        }*/
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (crouching)
        {
            return;
        }
        else { 
            if (sprinting)
            {
                speed = 12;
            }
            else
            {
                speed = 8;
            }
        }
    }
}
