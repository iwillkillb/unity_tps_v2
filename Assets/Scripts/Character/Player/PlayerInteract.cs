using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Interactable focus;
    public float range = 1f;

    // Update is called once per frame
    void Update()
    {
        // Left click -> Focus on Object.
        if (Input.GetMouseButtonDown(1))
        {
            Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitMouse;

            if (Physics.Raycast(rayMouse, out hitMouse))
            {
                // Interaction
                Interactable interactable = hitMouse.collider.transform.GetComponent<Interactable>();
                if (interactable != null && Vector3.Distance(transform.position, hitMouse.point) <= range)
                {
                    SetFocus(interactable);
                }
                else
                {
                    RemoveFocus();
                }
            }
        }
    }

    private void OnTriggerStay(Collider itemTrigger)
    {
        // Interaction
        Interactable interactable = itemTrigger.transform.GetComponent<Interactable>();
        if (interactable != null)
        {
            SetFocus(interactable);
        }
    }

    /*
    void LookAtPoint(Vector3 point)
    {
        Vector3 direction = (point - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }*/

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            // If there is already focus -> Disconnect.
            if (focus != null)
            {
                focus.OnDefocused();
            }
            // Set new focus.
            focus = newFocus;

        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }
}
