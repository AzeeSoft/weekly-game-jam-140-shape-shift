using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlightInput
{
    public float horizontal;
    public float vertical;
}

public enum HorizontalRotationMode
{
    Yaw,
    Roll
}

public class FlightController : MonoBehaviour
{
    [Header("References")]
    public Transform avatar;

    [Header("Movement")] public float flightMaxSpeed = 10f;
    public float flightAcceleration = 5f;
    public float flightDeceleration = 5f;
    public float flightLookStrength = 5f;
    public float flightLookAcceleration = 5f;
    [Range(0, 0.5f)] public float flightCameraBoundaryOffset = 0.1f;
    public HorizontalRotationMode horizontalRotationMode = HorizontalRotationMode.Roll;

    [Header("Shape Shifting")]
    public Material squareMaterial;
    public Material triangleMaterial;
    public Material circleMaterial;
    public Material starMaterial;

    [ReadOnly] public FlightInput flightInput = new FlightInput();

    [SerializeField] [ReadOnly] private Vector3 curVelocity;

    public FlightShape curFlightShape { get; private set; } = FlightShape.Circle;

    private FlightModel flightModel;
    private new Rigidbody rigidbody => flightModel.rigidbody;

    void Awake()
    {
        flightModel = GetComponent<FlightModel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateShape();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        ConfineWithinCameraBounds();
    }

    void Move()
    {
        var moveDir = (transform.right * flightInput.horizontal) + (transform.up * flightInput.vertical);
        moveDir.Normalize();

        var targetVelocity = moveDir * flightMaxSpeed;
        var curAcceleration =
            (targetVelocity.magnitude > curVelocity.magnitude) ? flightAcceleration : flightDeceleration;

        curVelocity = Vector3.Lerp(curVelocity, targetVelocity, Time.fixedDeltaTime * curAcceleration);

        rigidbody.velocity = curVelocity;

        /* var targetPos = transform.position + curVelocity;
        targetPos = GetViewportBoundedPosition(targetPos);

        // transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * flightLerpFactor);
        transform.position = targetPos;*/

        var rotationAngles = new Vector3(-moveDir.y, 0, 0);
        switch (horizontalRotationMode)
        {
            case HorizontalRotationMode.Roll:
                rotationAngles.z = -moveDir.x;
                break;
            case HorizontalRotationMode.Yaw:
                rotationAngles.y = moveDir.x;
                break;
        }

        var targetRot = Quaternion.Euler(rotationAngles * flightLookStrength);
        avatar.transform.rotation =
            Quaternion.Slerp(avatar.transform.rotation, targetRot, Time.fixedDeltaTime * flightLookAcceleration);
    }

    void ConfineWithinCameraBounds()
    {
        var targetPos = GetViewportBoundedPosition(transform.position);

        // transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * flightLerpFactor);
        transform.position = targetPos;
    }

    Vector3 GetViewportBoundedPosition(Vector3 targetPos)
    {
        var camera = CameraRig.Instance.cinemachineBrain.OutputCamera;
        var targetViewportPos = camera.WorldToViewportPoint(targetPos);
        Vector3 adjustedViewportPos =
            new Vector3(Mathf.Clamp(targetViewportPos.x, flightCameraBoundaryOffset, 1 - flightCameraBoundaryOffset),
                Mathf.Clamp(targetViewportPos.y, flightCameraBoundaryOffset, 1 - flightCameraBoundaryOffset),
                targetViewportPos.z);

        return camera.ViewportToWorldPoint(adjustedViewportPos);
    }

    public void ShapeShiftTo(FlightShape shape)
    {
        curFlightShape = shape;
        UpdateShape();
    }

    void UpdateShape()
    {
        var selectedMaterial = squareMaterial;
        switch (curFlightShape)
        {
            case FlightShape.Square:
                selectedMaterial = squareMaterial;
                break;
            case FlightShape.Triangle:
                selectedMaterial = triangleMaterial;
                break;
            case FlightShape.Circle:
                selectedMaterial = circleMaterial;
                break;
            case FlightShape.Star:
                selectedMaterial = starMaterial;
                break;
        }

        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material = selectedMaterial;
        }

        flightModel.FlightShip.UpdateShape();
    }
}