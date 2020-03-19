using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public string sceneToLoad;
    public TextMeshProUGUI mainMenuTextMesh;
    public TextMeshProUGUI highscoreTextMesh;
    public Text dummyText;
    public float textChangeSpeed;

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

        const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";

        for (int i = 0; i < 13; i++)
        {
            dummyText.text += glyphs[Random.Range(0, glyphs.Length)];
        }

        dummyText.DOText("Astral Angles", textChangeSpeed, true, ScrambleMode.All);
        
        Time.timeScale = 1;
    }

    void Update()
    {
        if (mainMenuTextMesh.text != "Astral Angles")
        {
            mainMenuTextMesh.text = dummyText.text;
        }
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
