using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : Interactable
{
    public Transform goalTransform;
    Vector3 startPoint;
    public float moveTemp = 0f;

    public float goalPoint;

    private void Start()
    {
        startPoint = transform.position;
    }

    private void FixedUpdate()
    {
        if (moveTemp >= Mathf.PI * 2f)
            moveTemp = 0f;
        else
            moveTemp += Time.deltaTime;

        goalPoint = (Mathf.Sin(moveTemp) + 1f) * 0.5f;

        transform.position = Vector3.Lerp(startPoint, goalTransform.position, goalPoint);
    }

    protected override void Interact()
    {
        Debug.Log("Vehicle On " + gameObject.name);

        interactor.parent = transform;
    }
}
