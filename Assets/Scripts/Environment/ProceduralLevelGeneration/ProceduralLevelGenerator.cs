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
        UpdateLevelUnits();
        SpawnLevelUnitsAsNeeded();
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