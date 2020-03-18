using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public string saveFile;
    public class GameData
    {
        public int highscore;
        public string highscorePlayerName;
    }

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
        GameData tempData = SaveSystem.LoadData<GameData>(saveFile);
        
        if (tempData == null)
        {
            tempData.highscore = 100;
            tempData.highscorePlayerName = "G.A.T.S";
            SaveSystem.SaveData<GameData>(tempData, saveFile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckandSetHighScore(int currentScore, string currentPlayerName)
    {
        int currentHighscore = SaveSystem.LoadData<GameData>(saveFile).highscore;
        
        if (currentHighscore < currentScore)
        {
            currentHighscore = currentScore;

            GameData tempData = new GameData
            {
                highscore = currentHighscore,
                highscorePlayerName = currentPlayerName
            };

            SaveSystem.SaveData<GameData>(tempData, saveFile);
        }
    }

    public GameData GetHighscoreData()
    {
        return SaveSystem.LoadData<GameData>(saveFile);
    }
}
