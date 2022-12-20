using Unity.Services.Core;
using UnityEngine;
using System;
using Unity.Netcode;

public class Initialize : NetworkBehaviour
{
public void Start() {

}
async void Awake()
	{
try
		{
await UnityServices.InitializeAsync();
		}
catch (Exception e)
		{
			Debug.LogException(e);
		}
	}
}
