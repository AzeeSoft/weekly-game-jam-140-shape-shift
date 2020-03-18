using BasicTools.ButtonInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrierBehavior : MonoBehaviour
{
    public Transform holeTransform;
    
    public float holeOffset;
    public float acccuracyOffset;

    private BoxCollider barrierCollider;

    [Header("Debug")]

    public Material[] possibleMaterials;

    [SerializeField]
    [Button("Get Hole Position", "InitializeBarrier")]
    private bool _btnHolePosition;

    public FlightShape flightShape;

    void Start()
    {
        barrierCollider = GetComponent<BoxCollider>();

        InitializeBarrier();
        debugChangeTransformColor();
    }

    private void InitializeBarrier()
    {
        holeTransform.position = new Vector3(Random.Range(barrierCollider.bounds.min.x + holeOffset, barrierCollider.bounds.max.x - holeOffset), Random.Range(barrierCollider.bounds.min.y + holeOffset, barrierCollider.bounds.max.y - holeOffset), Random.Range(barrierCollider.bounds.min.z + holeOffset, barrierCollider.bounds.max.z - holeOffset));

        string[] ListOfFlightShapes = Enum.GetNames(typeof(FlightShape));
        flightShape = (FlightShape)(Random.Range(0, ListOfFlightShapes.Length));
    }

    private void debugChangeTransformColor()
    {
        Material currentMaterial = possibleMaterials[(int)flightShape % possibleMaterials.Length];

        holeTransform.gameObject.GetComponent<MeshRenderer>().material = currentMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        FlightModel otherFM = other.gameObject.GetComponent<FlightModel>();

        if (otherFM != null)
        {
            if (Vector3.Distance(holeTransform.position, other.gameObject.transform.position) <= acccuracyOffset && otherFM.curFlightShape == flightShape)
            {
                print("Succesfully passed through barrier!");
            }
            else
            {
                print("Ouch! Missed the Hole / Wrong shape!");
            }
        }
    }
}