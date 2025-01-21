using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // message displayed to player when looking at an interactable
    public string promptMessage;

    // this function will be called from our player
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        // we won't have any code written in this function
        // this is a template function to be overridden by our subclasses
    }
}