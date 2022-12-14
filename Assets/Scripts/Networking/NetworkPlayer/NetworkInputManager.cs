using UnityEngine;
using Unity.Netcode;

public class NetworkInputManager : NetworkBehaviour {
    public override void OnNetworkSpawn() {
        if(!IsOwner) {this.enabled = false;}
    }

    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;

    private NetworkPlayerMotor motor;

    private NetworkPlayerLook look;

    // Start is called before the first frame update
    void Awake() {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<NetworkPlayerMotor>();
        look = GetComponent<NetworkPlayerLook>();

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
