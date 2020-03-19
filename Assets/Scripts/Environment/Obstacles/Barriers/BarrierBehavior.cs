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
    public float damage = 20;
    public float refill = 10;

    public BoxCollider barrierCollider;

    public AudioClip hitSound;
    public AudioClip refillSound;

    public GameObject[] holePrefabs;

    /*[Header("Debug")]

    public Material[] possibleMaterials;*/

    [SerializeField]
    [Button("Get Hole Position", "InitializeBarrier")]
    private bool _btnHolePosition;

    private float[] transformPlacementCoords;

    void Start()
    {
        barrierCollider = GetComponent<BoxCollider>();

        transformPlacementCoords = new float[] {barrierCollider.bounds.min.x, barrierCollider.bounds.max.x, barrierCollider.bounds.min.y, barrierCollider.bounds.max.y, barrierCollider.bounds.min.z, barrierCollider.bounds.max.z};

        InitializeBarrier();
        UpdateHole();
    }

    void OnEnable()
    {
        if (autoGenerateBarrierOnEnable)
        {
            InitializeBarrier();
            UpdateHole();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(holeTransform.position, acccuracyOffset);

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

    private void UpdateHole()
    {
        /*Material currentMaterial = possibleMaterials[(int)flightShape % possibleMaterials.Length];
        holeTransform.gameObject.GetComponent<MeshRenderer>().material = currentMaterial;*/

        holeTransform.DestroyAllChildren();

        var hole = Instantiate(holePrefabs[(int) flightShape % holePrefabs.Length], holeTransform);
        hole.transform.localPosition = Vector3.zero;
        hole.transform.localRotation = Quaternion.identity;
    }

    void OnTriggerEnter(Collider other)
    {
        FlightModel otherFM = other.gameObject.GetComponentInParent<FlightModel>();

        if (otherFM != null)
        {
            var adjustedFlightPos = otherFM.transform.position;
            // adjustedFlightPos.z = holeTransform.position.z;

            if (Vector3.Distance(holeTransform.position, adjustedFlightPos) <= acccuracyOffset && otherFM.curFlightShape == flightShape)
            {
                print("Succesfully passed through barrier!");
                otherFM.health.UpdateHealth(refill);
                otherFM.PassedThroughBarrier(this, true);

                SoundEffectsManager.Instance.Play(refillSound);
            }
            else
            {
                print("Ouch! Missed the Hole / Wrong shape!");
                print("Dist: " + Vector3.Distance(holeTransform.position, adjustedFlightPos));
                print("Flight Shape: " + otherFM.curFlightShape);
                print("Hole Shape: " + flightShape);
                otherFM.health.TakeDamage(damage);
                otherFM.PassedThroughBarrier(this, false);

                SoundEffectsManager.Instance.Play(hitSound);
            }
        }
    }
}