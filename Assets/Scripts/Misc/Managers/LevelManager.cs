using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public int score;

    [Header("References")]
    public FlightModel flightModel;
    public ProceduralLevelGenerator proceduralLevelGenerator;

    public void UpdateHighScore(string playerName = "Anonymous")
    {
        GameManager.Instance.CheckAndSetHighScore(score, playerName);
    }

}
