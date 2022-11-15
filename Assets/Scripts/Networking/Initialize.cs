using Unity.Services.Core;
using UnityEngine;
using System;

public class Initialize : MonoBehaviour
{
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
