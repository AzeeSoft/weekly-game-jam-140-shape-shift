using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public string sceneToLoad;
    public TextMeshProUGUI highscoreTextMesh;

    Sequence textColorSequence;
    public float colorChangeSpeed;
    public Color textColor1;
    public Color textColor2;

    void Start()
    {
        GameManager.GameData gameData = SaveSystem.LoadData<GameManager.GameData>(GameManager.Instance.saveFile);
        highscoreTextMesh.text = $"Highscore: {gameData.highscorePlayerName} {gameData.highscore}";
        highscoreTextMesh.color = textColor1;

        textColorSequence = DOTween.Sequence();
        textColorSequence.Append(highscoreTextMesh.DOColor(textColor2, colorChangeSpeed)).Append(highscoreTextMesh.DOColor(textColor1, colorChangeSpeed)).SetLoops(-1);

        Time.timeScale = 1;
    }

    public void loadNextScene()
    {
        ScreenFader.Instance.FadeOut(-1, () => { SceneManager.LoadScene(sceneToLoad); });
    }

    public void quitGame()
    {
        Application.Quit();
        print("Quitting game.");
    }
}
