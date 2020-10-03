using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Transform parentBackup;

    Animator animator;
    Collider[] boneColliders;
    Rigidbody[] boneRigidbodies;
    bool onRagdoll;

    // Start is called before the first frame update
    void Start()
    {
        parentBackup = transform.parent;

        animator = GetComponent<Animator>();
        boneColliders = GetComponentsInChildren<Collider>();
        boneRigidbodies = GetComponentsInChildren<Rigidbody>();
        ToggleRagdoll(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            onRagdoll = !onRagdoll;
            ToggleRagdoll(onRagdoll);
        }
    }

    void ToggleRagdoll(bool state)
    {
        // Ragdoll -> Animator off
        animator.enabled = !state;

        parentBackup.GetComponent<PlayerMovementCC>().enabled = !state;
        parentBackup.GetComponent<CharacterController>().enabled = !state;

        // Parent setting : Don't chase parent's movement.
        // Position reset
        if (state)
        {
            // Be Ragdoll
            transform.parent = null;
        }
        else
        {
            // Return
            transform.parent = parentBackup;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        // Collider and Rigidbody setting
        foreach (Rigidbody rigi in boneRigidbodies)
        {
            rigi.isKinematic = !state;

            if (state)
            {
                rigi.AddExplosionForce(10f, transform.position + Vector3.up, 1f, 0f, ForceMode.VelocityChange);
                //rigi.AddForce(parentBackup.forward * 10f, ForceMode.VelocityChange);
            }
        }
        foreach (Collider bone in boneColliders)
        {
            bone.enabled = state;
        }
    }
}
