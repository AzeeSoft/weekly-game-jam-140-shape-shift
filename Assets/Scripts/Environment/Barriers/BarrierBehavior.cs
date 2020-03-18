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
    public FlightShape flightShape;
    public bool autoGenerateBarrierOnEnable = false;

    public BoxCollider barrierCollider;

    [Header("Debug")]

    public Material[] possibleMaterials;

    [SerializeField]
    [Button("Get Hole Position", "InitializeBarrier")]
    private bool _btnHolePosition;

    private float[] transformPlacementCoords;

    void Start()
    {
        barrierCollider = GetComponent<BoxCollider>();

        transformPlacementCoords = new float[] {barrierCollider.bounds.min.x, barrierCollider.bounds.max.x, barrierCollider.bounds.min.y, barrierCollider.bounds.max.y, barrierCollider.bounds.min.z, barrierCollider.bounds.max.z};

        InitializeBarrier();
        debugChangeTransformColor();
    }

    void OnEnable()
    {
        if (autoGenerateBarrierOnEnable)
        {
            InitializeBarrier();
            debugChangeTransformColor();
        }
    }

    private void InitializeBarrier()
    {
        float x, y;
        x = barrierCollider.size.x / 2;
        y = barrierCollider.size.y / 2;

        holeTransform.localPosition = new Vector3(Random.Range(-x + holeOffset, x - holeOffset), Random.Range(-y + holeOffset, y - holeOffset), 0);
            
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