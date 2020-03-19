using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public float scoreBaseIncreaseRate = 10;
    public int maxScoreMultiplier = 10;

    public float score { get; private set; } = 0;

    public int scoreMultiplier
    {
        get => _scoreMultiplier;

        private set
        {
            _scoreMultiplier = Mathf.Clamp(value, 1, maxScoreMultiplier);
            proceduralLevelGenerator.additiveSpeedFactor =
                HelperUtilities.Remap(_scoreMultiplier, 1, maxScoreMultiplier, 0, 1);
        }
    }

    private int _scoreMultiplier = 1;

    [Header("References")]
    public FlightModel flightModel;
    public ProceduralLevelGenerator proceduralLevelGenerator;

    new void Awake()
    {
        base.Awake();

        Time.timeScale = 1f;
        HelperUtilities.UpdateCursorLock(true);
    }

    void Start()
    {
        flightModel.onPassedThroughBarrier += (barrier, success) =>
        {
            if (success)
            {
                scoreMultiplier++;
                scoreMultiplier = Mathf.Clamp(scoreMultiplier, 1, maxScoreMultiplier);
            }
        };

        flightModel.health.OnDamageTaken.AddListener(() => { scoreMultiplier = 1; });
    }

    void Update()
    {
        score += scoreBaseIncreaseRate * Time.deltaTime * scoreMultiplier;
    }

    public void UpdateHighScore(string playerName = "Anonymous")
    {
        GameManager.Instance.CheckAndSetHighScore((int) score, playerName);
    }
}
