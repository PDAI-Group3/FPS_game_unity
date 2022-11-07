using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] private LayerMask playerMask;

    private Rigidbody rigidbodyComponent;

    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private float verticalInput;
    public float movementSpeed = 5;

    public Camera eyes;
    private float mouseX;
    private float mouseY;
    public float sensitivity;
    
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        // Jump key pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Camera rotating with mouse
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(0, mouseX*sensitivity, 0);
        eyes.transform.Rotate(-mouseY*sensitivity, 0, 0);
        
    }

    // FixedUpdate is called once every physics update
    private void FixedUpdate()
    {
        // Movement (WASD)
        Vector3 movement = new Vector3(horizontalInput * movementSpeed, rigidbodyComponent.velocity.y, verticalInput * movementSpeed);
        rigidbodyComponent.velocity = transform.rotation*movement;

        // Checking if the player is in the air
        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }
       

        if (jumpKeyWasPressed == true)
        {
            rigidbodyComponent.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
            jumpKeyWasPressed = false;
        }

    }

   
}

