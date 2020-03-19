using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreMultiplierText;

    public float prevHighScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        prevHighScore = GameManager.Instance.GetGameData().highscore;

        //LevelManager.Instance.planetHealth.OnDamageTaken.AddListener(IndicateDamage);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthBar.fillAmount = LevelManager.Instance.flightModel.health.normalizedHealth;
        healthText.text = $"{LevelManager.Instance.flightModel.health.currentHealth:##0}";
        highscoreText.text = $"Highscore: {Mathf.Max(LevelManager.Instance.score, prevHighScore):###,###,##0}";
        scoreText.text = $"Score: {LevelManager.Instance.score:###,###,##0}";
        scoreMultiplierText.text = $"x{LevelManager.Instance.scoreMultiplier:##0}";
    }
}
