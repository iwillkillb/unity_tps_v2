using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIKTest : MonoBehaviour
{
    public Animator _Animator;
    public bool ikActive;
    [Range(0f, 1f)]public float ikWeight = 1f;
    public Transform leftHandObj;
    public Transform rightHandObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //hg

        if (_Animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {
                // Set the left hand target position and rotation, if one has been assigned
                _Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
                _Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);
                _Animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                _Animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);

                // Set the right hand target position and rotation, if one has been assigned
                _Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
                _Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
                _Animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                _Animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);

                // Set the look target position, if one has been assigned
                _Animator.SetLookAtWeight(ikWeight);
                _Animator.SetLookAtPosition(Vector3.Lerp(leftHandObj.position, rightHandObj.position, 0.5f));



            }
        }
    }
}
