using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Input field
    public float axisHor { get; set; }
    public float axisVer { get; set; }
    public bool axisJump { get; set; }
    public bool axisAttack { get; set; }
    public bool axisInteract { get; set; }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        axisHor = Input.GetAxis("Horizontal");
        axisVer = Input.GetAxis("Vertical");
        axisJump = Input.GetButtonDown("Jump");
        axisAttack = Input.GetMouseButton(0);
        axisInteract = Input.GetMouseButton(1);
    }
}
