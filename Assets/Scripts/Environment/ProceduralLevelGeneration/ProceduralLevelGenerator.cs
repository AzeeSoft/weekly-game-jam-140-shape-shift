using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ProceduralLevelZone
{
    public int scoreRange = 1000;
    [HideInInspector] public int cumulativeScoreRange = 0;

    public List<GameObject> proceduralLevelUnitPrefabs;
    public Randomizer<GameObject> proceduralLevelUnitRandomizer;

    public void Init()
    {
        proceduralLevelUnitRandomizer = new Randomizer<GameObject>(proceduralLevelUnitPrefabs);
    }
}

public class ProceduralLevelGenerator : MonoBehaviour
{
    public const int maxLevelUnitsSpawnedPerFrame = 100;

    public float startOffset = 10f;
    public float endOffset = 500f;
    public Range speedRange;
    [Range(0, 1)] public float additiveSpeedFactor;

    public Transform levelUnitsHolder;

    public GameObject emptyLevelUnitPrefab;
    public int emptyLevelUnitBuffer = 5;
    public List<ProceduralLevelZone> proceduralLevelZones;

    [Header("Depth Curvature Settings")] [Range(-1, 1)]
    public float curve = 0;

    public float maxCurve = 0;
    [Range(0, 1)] public float minDepthForCurvature = 0;
    public Range curveSpeedRange;
    public bool autoSwitchCurveTarget = false;
    public Range curveTargetDurationRange;

    public float curSpeed => HelperUtilities.Remap(additiveSpeedFactor, 0, 1, speedRange.min, speedRange.max);

    private float curveSource = 0;
    private float curveTarget = 0;
    private float curCurveSpeed = 0;
    private float curCurveTargetDuration = 0;
    private float timeSinceCurveTargetSet = 0;
    private int emptyLevelUnitsAdded = 0;

    private int curLevelZoneIndex = 0;
    private ProceduralLevelZone curProceduralLevelZone => proceduralLevelZones[curLevelZoneIndex];

    private int curStencilRef = 1;

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
        int cumulativeScoreRange = 0;
        for (var i = 0; i < proceduralLevelZones.Count; i++)
        {
            proceduralLevelZones[i].Init();

            cumulativeScoreRange += proceduralLevelZones[i].scoreRange;
            proceduralLevelZones[i].cumulativeScoreRange = cumulativeScoreRange;
        }

        curLevelZoneIndex = 0;
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
        UpdateLevelZone();
    }

    void UpdateDepthCurvatureEffect()
    {
        timeSinceCurveTargetSet += Time.deltaTime;

        if (autoSwitchCurveTarget)
        {
            CheckAndSwitchCurvetarget();
        }

        curve = Mathf.Lerp(curve, curveTarget, Time.deltaTime * curCurveSpeed);

        Shader.SetGlobalFloat("DepthCurvatureCurve", HelperUtilities.Remap(curve, -1, 1, -maxCurve, maxCurve));
        Shader.SetGlobalFloat("DepthCurvatureMinDepth", minDepthForCurvature);
    }

    public void CheckAndSwitchCurvetarget()
    {
        if (timeSinceCurveTargetSet >= curCurveTargetDuration)
        {
            curveTarget = Random.Range(-1f, 1f);
            curCurveSpeed = curveSpeedRange.GetRandomInRange();
            curCurveTargetDuration = curveTargetDurationRange.GetRandomInRange();
            timeSinceCurveTargetSet = 0;
            curveSource = curve;
        }
    }

    void UpdateLevelUnits()
    {
        var moveDelta = -transform.forward * curSpeed * Time.deltaTime;

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
            GameObject selectedPrefab;
            if (emptyLevelUnitsAdded < emptyLevelUnitBuffer)
            {
                selectedPrefab = emptyLevelUnitPrefab;
                emptyLevelUnitsAdded++;
            }
            else
            {
                selectedPrefab = curProceduralLevelZone.proceduralLevelUnitRandomizer.GetRandomItem();
            }

            var proceduralLevelUnit = Instantiate(selectedPrefab, levelUnitsHolder).GetComponent<ProceduralLevelUnit>();
            proceduralLevelUnit.transform.position = nextSpawnPoint;
            proceduralLevelUnit.transform.position +=
                proceduralLevelUnit.transform.position - proceduralLevelUnit.prevConnector.position;

            foreach (var barrierBehavior in proceduralLevelUnit.GetComponentsInChildren<BarrierBehavior>())
            {
                barrierBehavior.UpdateStencilRef(curStencilRef);

                curStencilRef = Mathf.Clamp((curStencilRef + 1) % 256, 1, 255);
            }

            proceduralLevelUnits.Add(proceduralLevelUnit);

            nextSpawnPoint = proceduralLevelUnit.nextConnector.position;

            levelUnitsSpawned++;
        }
    }

    void UpdateLevelZone()
    {
        if (LevelManager.Instance)
        {
            if (LevelManager.Instance.score > curProceduralLevelZone.cumulativeScoreRange &&
                (curLevelZoneIndex < proceduralLevelZones.Count - 1))
            {
                curLevelZoneIndex++;
            }
        }
    }
}