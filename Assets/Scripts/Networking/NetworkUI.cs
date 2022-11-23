using Unity.Netcode;
using UnityEngine;
using UnityEditor;

public class NetworkUI : MonoBehaviour {
    private void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        GUILayout.TextField(HostAndJoin.joinCode);
        GUILayout.EndArea();
    }
}
