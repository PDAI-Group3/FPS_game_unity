using UnityEngine;
using TMPro;
using System;

public class SaveJoinCode : MonoBehaviour
{
    public TMP_InputField JoinCode;

    public void Start() {
        JoinCode.onValueChanged.AddListener(SaveCode);
    }

    public void SaveCode(string data)
    {
        SceneLoader.joinCode = JoinCode.text.ToString();
    }
}