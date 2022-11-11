using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerLook : NetworkBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    // Start is called before the first frame update
    void Start()
    {
        // IF I'M THE PLAYER, STOP HERE (DON'T TURN MY OWN CAMERA OFF)
        if (IsLocalPlayer) return;
 
        // DISABLE CAMERA AND CONTROLS HERE (BECAUSE THEY ARE NOT ME)
        cam.enabled = false;
        //GetComponent<PlayerControls>().enabled = false;
        //GetComponent<PlayerMovement>().enabled = false;
    }
    public void ProcessLook(Vector2 input)
    {
        float mouseX =input.x;
        float mouseY =input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
