using Unity.Netcode;
using UnityEngine;

public class Door : NetworkBehaviour {
    public NetworkVariable<bool> State = new NetworkVariable<bool>();

    [SerializeField]
    private GameObject door;

    public override void OnNetworkSpawn()
    {
        State.OnValueChanged += OnStateChanged;
    }

    public override void OnNetworkDespawn()
    {
        State.OnValueChanged -= OnStateChanged;
    }

    public void OnStateChanged(bool previous, bool current)
    {
        // note: `State.Value` will be equal to `current` here
        if (State.Value)
        {
        door.GetComponent<Animator>().SetBool("IsOpen", true);
        }
        else
        {
        door.GetComponent<Animator>().SetBool("IsOpen", false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DoorToggleServerRpc()
    {
        // this will cause a replication over the network
        // and ultimately invoke `OnValueChanged` on receivers
        State.Value = !State.Value;
    }
}
