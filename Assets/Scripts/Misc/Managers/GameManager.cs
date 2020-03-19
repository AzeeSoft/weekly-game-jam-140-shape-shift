using System.Collections;
using System.Collections.Generic;
using BasicTools.ButtonInspector;
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
        public PlayerSettings playerSettings = new PlayerSettings();
    }

    public GameData gameData { get; private set; }
    public string saveFile;

    [SerializeField] [Button("Reset Game Data", "ResetGameData")]
    private bool _btnResetGameData;

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
        gameData = SaveSystem.LoadData<GameData>(saveFile);
        
        if (gameData == null)
        {
            ResetGameData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAndSetHighScore(int currentScore, string currentPlayerName)
    {
        if (gameData.highscore < currentScore)
        {
            gameData.highscore = currentScore;

            gameData.highscorePlayerName = currentPlayerName;

            SaveGameData();
        }
    }

    public void SaveGameData()
    {
        SaveSystem.SaveData<GameData>(gameData, saveFile);
    }

    public GameData GetGameData()
    {
        return gameData;
    }

    public void ResetGameData()
    {
        gameData = new GameData
        {
            highscore = 100,
            highscorePlayerName = "G.A.T.S"
        };

        SaveGameData();
    }
}
