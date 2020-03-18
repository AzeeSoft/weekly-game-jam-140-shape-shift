using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlightInput
{
    public float horizontal;
    public float vertical;
    public FlightShape? shapeShiftTo;
}

public class FlightController : MonoBehaviour
{
    [Header("References")] public Transform avatar;

    [Header("Movement")]
    public float flightMaxSpeed = 10f;
    public float flightAcceleration = 5f;
    public float flightDeceleration = 5f;
    public float flightLookStrength = 5f;
    public float flightLookAcceleration = 5f;

    [ReadOnly] public FlightInput flightInput = new FlightInput();

    [SerializeField] [ReadOnly] private Vector3 curVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        var moveDir = (transform.right * flightInput.horizontal) + (transform.up * flightInput.vertical);
        moveDir.Normalize();

        var targetVelocity = moveDir * flightMaxSpeed;
        var curAcceleration =
            (targetVelocity.magnitude > curVelocity.magnitude) ? flightAcceleration : flightDeceleration;

        curVelocity = Vector3.Lerp(curVelocity, targetVelocity, Time.deltaTime * curAcceleration);

        var targetPos = transform.position + curVelocity;

        // transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * flightLerpFactor);
        transform.position = targetPos;

        var rotationAngles = new Vector3(-moveDir.y, moveDir.x, 0);
        var targetRot = Quaternion.Euler(rotationAngles * flightLookStrength);
        avatar.transform.rotation = Quaternion.Slerp(avatar.transform.rotation, targetRot, Time.deltaTime * flightLookAcceleration);
    }
}
