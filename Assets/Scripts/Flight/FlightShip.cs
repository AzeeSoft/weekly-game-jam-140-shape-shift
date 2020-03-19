using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FlightShip : MonoBehaviour
{
    public List<SkinnedMeshRenderer> skinnedMeshRenderers;
    public float blendDuration = 1f;

    public SkinnedMeshRenderer hullMeshRenderer;
    public GameObject thrusterPrefab;
    public Transform thrusterHolder;
    public Vector3 thrusterOffset = Vector3.zero;

    public List<int> thrusterSubMeshIndices;

    public static readonly Dictionary<FlightShape, int> flightBlendShapeIndices = new Dictionary<FlightShape, int>()
    {
        {FlightShape.Star, 0},
        {FlightShape.Triangle, 1},
        {FlightShape.Square, 2},
        {FlightShape.Circle, 3},
    };

    private FlightModel flightModel;
    private FlightShape curFlightShape = FlightShape.Circle;
    private List<GameObject> thrusters = new List<GameObject>();

    void Awake()
    {
        flightModel = GetComponentInParent<FlightModel>();
        curFlightShape = flightModel.curFlightShape;
        GenerateThrusters();

        print("OG Sub Meshes: " + hullMeshRenderer.sharedMesh.subMeshCount);
        for (int i = 0; i < hullMeshRenderer.sharedMesh.subMeshCount; i++)
        {
            print(
                $"OG Sub Mesh {i} Center: {hullMeshRenderer.sharedMesh.GetSubMesh(i).bounds.center}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateThrusters();
    }

    public void UpdateShape()
    {
        var newFlightShape = flightModel.curFlightShape;

        if (newFlightShape == curFlightShape)
        {
            return;
        }

        foreach (var wingsMeshRenderer in skinnedMeshRenderers)
        {
            // wingsMeshRenderer.DOKill(true);
            wingsMeshRenderer.DOBlendShapeWeight(flightBlendShapeIndices[curFlightShape], 0, blendDuration).Play();
            wingsMeshRenderer.DOBlendShapeWeight(flightBlendShapeIndices[newFlightShape], 100, blendDuration).Play();
        }

        curFlightShape = newFlightShape;
    }

    public void GenerateThrusters()
    {
        thrusters.Clear();
        thrusterHolder.DestroyAllChildren();

        foreach (var subMeshIndex in thrusterSubMeshIndices)
        {
            var thruster = Instantiate(thrusterPrefab, thrusterHolder);
            thruster.transform.localPosition = Vector3.zero;
            thruster.transform.localRotation = Quaternion.identity;

            thrusters.Add(thruster);
        }
    }

    void UpdateThrusters()
    {
        Mesh mesh = new Mesh();
        hullMeshRenderer.BakeMesh(mesh);
        mesh.RecalculateBounds();

        for (int i = 0; i < thrusters.Count; i++)
        {
            int submeshIndex = thrusterSubMeshIndices[i];
            var subMesh = mesh.GetSubMesh(submeshIndex);
            int[] submeshTriangles = mesh.GetTriangles(submeshIndex);
            thrusters[i].transform.position =
                hullMeshRenderer.transform.TransformPoint(subMesh.bounds.center);
            thrusters[i].transform.localPosition += thrusterOffset;

            print(
                $"OG Sub Mesh {thrusterSubMeshIndices[i]} Center: {mesh.vertices[subMesh.firstVertex]}");
        }
    }
}