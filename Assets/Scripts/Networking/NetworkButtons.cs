using Unity.Netcode;
using UnityEngine;
using UnityEditor;

public class NetworkButtons : MonoBehaviour {
    private void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        HostAndJoin.joinCode = GUILayout.TextField(HostAndJoin.joinCode);

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
            if (GUILayout.Button("Host")) {
                HostAndJoin.HostGame(4);
            }
        }

        if (!NetworkManager.Singleton.IsHost && !NetworkManager.Singleton.IsServer) {
            if (GUILayout.Button("Client")) HostAndJoin.JoinGame(HostAndJoin.joinCode);
        }

        GUILayout.EndArea();
    }
}
