using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralLevelGenerator : MonoBehaviour
{
    public const int maxLevelUnitsSpawnedPerFrame = 100;

    public float startOffset = 10f;
    public float endOffset = 500f;
    public float speed = 5f;

    public Transform levelUnitsHolder;

    public List<GameObject> proceduralLevelUnitPrefabs;

    [Header("Depth Curvature Settings")]
    [Range(-1, 1)] public float curve = 0;
    public float maxCurve = 0;
    [Range(0, 1)] public float minDepthForCurvature = 0;
    public Range curveSpeedRange;
    public Range curveTargetDurationRange;

    private float curveSource = 0;
    private float curveTarget = 0;
    private float curCurveSpeed = 0;
    private float curCurveTargetDuration = 0;
    private float timeSinceCurveTargetSet = 0;

    private Randomizer<GameObject> proceduralLevelUnitRandomizer;
    private List<ProceduralLevelUnit> proceduralLevelUnits = new List<ProceduralLevelUnit>();

    void OnDrawGizmos()
    {
        float pointRadius = 5f;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + (-transform.forward * startOffset), pointRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (transform.forward * endOffset), pointRadius);
    }

    void Awake()
    {
        proceduralLevelUnitRandomizer = new Randomizer<GameObject>(proceduralLevelUnitPrefabs);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDepthCurvatureEffect();
        UpdateLevelUnits();
        SpawnLevelUnitsAsNeeded();
    }

    void UpdateDepthCurvatureEffect()
    {
        timeSinceCurveTargetSet += Time.deltaTime;

        if (timeSinceCurveTargetSet >= curCurveTargetDuration)
        {
            curveTarget = Random.Range(-1f, 1f);
            curCurveSpeed = curveSpeedRange.GetRandomInRange();
            curCurveTargetDuration = curveTargetDurationRange.GetRandomInRange();
            timeSinceCurveTargetSet = 0;
            curveSource = curve;
        }

        curve = Mathf.Lerp(curve, curveTarget, Time.deltaTime * curCurveSpeed);

        Shader.SetGlobalFloat("DepthCurvatureCurve", HelperUtilities.Remap(curve, -1, 1, -maxCurve, maxCurve));
        Shader.SetGlobalFloat("DepthCurvatureMinDepth", minDepthForCurvature);
    }

    void UpdateLevelUnits()
    {
        var moveDelta = -transform.forward * speed * Time.deltaTime;

        HashSet<ProceduralLevelUnit> levelUnitsToRemove = new HashSet<ProceduralLevelUnit>();

        foreach (var proceduralLevelUnit in proceduralLevelUnits)
        {
            proceduralLevelUnit.transform.position += moveDelta;

            if (proceduralLevelUnit.nextConnector.position.z < transform.position.z &&
                Vector3.Distance(transform.position, proceduralLevelUnit.nextConnector.position) > startOffset)
            {
                Destroy(proceduralLevelUnit.gameObject);
                levelUnitsToRemove.Add(proceduralLevelUnit);
            }
        }

        proceduralLevelUnits.RemoveAll(levelUnit => levelUnitsToRemove.Contains(levelUnit));
    }

    void SpawnLevelUnitsAsNeeded()
    {
        var nextSpawnPoint = transform.position + (-transform.forward * startOffset);
        if (proceduralLevelUnits.Count > 0)
        {
            nextSpawnPoint = proceduralLevelUnits[proceduralLevelUnits.Count - 1].nextConnector.position;
        }

        int levelUnitsSpawned = 0;

        while ((nextSpawnPoint.z < transform.position.z ||
                Vector3.Distance(transform.position, nextSpawnPoint) < endOffset) &&
               levelUnitsSpawned < maxLevelUnitsSpawnedPerFrame)
        {
            var proceduralLevelUnit = Instantiate(proceduralLevelUnitRandomizer.GetRandomItem(), levelUnitsHolder)
                .GetComponent<ProceduralLevelUnit>();
            proceduralLevelUnit.transform.position = nextSpawnPoint;
            proceduralLevelUnit.transform.position +=
                proceduralLevelUnit.transform.position - proceduralLevelUnit.prevConnector.position;
            proceduralLevelUnits.Add(proceduralLevelUnit);

            nextSpawnPoint = proceduralLevelUnit.nextConnector.position;

            levelUnitsSpawned++;
        }
    }
}