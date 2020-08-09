using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableActive : Interactable
{
    public GameObject[] interactableObjects;

    protected override void Interact()
    {
        base.Interact();

        foreach (GameObject interactableObject in interactableObjects)
        {
            interactableObject.SetActive(!interactableObject.activeSelf);
        }
    }
}
