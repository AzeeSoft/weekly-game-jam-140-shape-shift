using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FlightShip : MonoBehaviour
{
    public List<SkinnedMeshRenderer> wingsMeshRenderers;
    public float blendDuration = 1f;

    public static readonly Dictionary<FlightShape, int> flightBlendShapeIndices = new Dictionary<FlightShape, int>()
    {
        {FlightShape.Star, 0},
        {FlightShape.Triangle, 1},
        {FlightShape.Square, 2},
        {FlightShape.Circle, 3},
    };

    private FlightModel flightModel;
    private FlightShape curFlightShape = FlightShape.Circle;

    void Awake()
    {
        flightModel = GetComponentInParent<FlightModel>();
        curFlightShape = flightModel.curFlightShape;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateShape()
    {
        var newFlightShape = flightModel.curFlightShape;

        if (newFlightShape == curFlightShape)
        {
            return;
        }

        foreach (var wingsMeshRenderer in wingsMeshRenderers)
        {
            // wingsMeshRenderer.DOKill(true);
            wingsMeshRenderer.DOBlendShapeWeight(flightBlendShapeIndices[curFlightShape], 0, blendDuration).Play();
            wingsMeshRenderer.DOBlendShapeWeight(flightBlendShapeIndices[newFlightShape], 100, blendDuration).Play();
        }

        curFlightShape = newFlightShape;
    }
}