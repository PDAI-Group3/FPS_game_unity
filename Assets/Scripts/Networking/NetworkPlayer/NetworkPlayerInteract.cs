using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayerInteract : NetworkBehaviour
{
    private Camera cam;

    [SerializeField] 
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private NetworkInputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<NetworkPlayerLook>().cam;
        //playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<NetworkInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; //storing collision information
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                //playerUI.UpdateText(interactable.promptMessage);
                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}
