using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    //displayed message when looking at interactable
    public string promptMessage;

    //this function will be called from player
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        //this function is only a template to be overridden by subclasses
    }
}
