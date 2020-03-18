using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [System.Serializable]
    public class PlayerSettings
    {
        public bool shouldInvertY;
    }

    [System.Serializable]
    public class GameData
    {
        public int highscore;
        public string highscorePlayerName;
        public PlayerSettings playerSettings;
    }

    public string saveFile;

    new void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameData currentData = SaveSystem.LoadData<GameData>(saveFile);
        
        if (currentData == null)
        {
            GameData tempData = new GameData
            {
                highscore = 100,
                highscorePlayerName = "G.A.T.S"
            };

            SaveSystem.SaveData<GameData>(tempData, saveFile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAndSetHighScore(int currentScore, string currentPlayerName)
    {
        GameData currentHighscoreData = SaveSystem.LoadData<GameData>(saveFile);
        
        if (currentHighscoreData.highscore < currentScore)
        {
            currentHighscoreData.highscore = currentScore;

            currentHighscoreData.highscorePlayerName = currentPlayerName;

            SaveSystem.SaveData<GameData>(currentHighscoreData, saveFile);
        }
    }

    public GameData GetHighscoreData()
    {
        return SaveSystem.LoadData<GameData>(saveFile);
    }
}
