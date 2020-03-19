using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public GameObject root;
    public TextMeshProUGUI yourScore;
    public TextMeshProUGUI highScore;

    public MenuPage endGameOptionsPage;
    public MenuPage newHighScorePage;
    public TMP_InputField highScorePlayerName;

    public AudioClip gameOverClip;

    private float transitionDuration = 1f;

    void Awake()
    {
        root.DOFade(0, 0).Play();
        endGameOptionsPage.gameObject.SetActive(true);
        newHighScorePage.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelManager.Instance.flightModel.health.OnHealthDepleted.AddListener(ShowEndScreen);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ShowEndScreen()
    {
        SoundEffectsManager.Instance.Play(gameOverClip);

        Time.timeScale = 0f;
        HelperUtilities.UpdateCursorLock(false);

        if (LevelManager.Instance.score > GameManager.Instance.gameData.highscore)
        {
            endGameOptionsPage.gameObject.SetActive(false);
            newHighScorePage.gameObject.SetActive(true);
        }

        highScore.text =
            $"Highscore: {Mathf.Max(LevelManager.Instance.score, GameManager.Instance.gameData.highscore):###,###,##0}";
        yourScore.text = $"Your Score: {LevelManager.Instance.score:###,###,##0}";

        root.SetActive(true);
        root.DOFade(1, transitionDuration).SetUpdate(true).Play();
    }

    public void Restart()
    {
        GameManager.Instance.RestartCurrentScene();
    }

    public void MainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }

    public void UpdateHighScore()
    {
        var name = highScorePlayerName.text.Trim();
        if (name.Length > 0)
        {
            LevelManager.Instance.UpdateHighScore(name);
        }
        
        newHighScorePage.Hide();
        endGameOptionsPage.Show();
    }
}