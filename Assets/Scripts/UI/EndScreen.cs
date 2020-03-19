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

    private float transitionDuration = 1f;

    void Awake()
    {
        root.DOFade(0, 0).Play();
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
        Time.timeScale = 0f;
        HelperUtilities.UpdateCursorLock(false);

        LevelManager.Instance.UpdateHighScore();

        highScore.text = $"Highscore: {Mathf.Max(LevelManager.Instance.score, GameManager.Instance.gameData.highscore):###,###,##0}";
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
}
