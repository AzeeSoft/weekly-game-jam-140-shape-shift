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

    public Image damageIndicatorImage;
    public float damageIndicatorMaxOpacity = 0.7f;
    public float damageIndicatorAnimationDuration = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        //LevelManager.Instance.planetHealth.OnDamageTaken.AddListener(IndicateDamage);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        /*healthBar.fillAmount = LevelManager.Instance.planetHealth.normalizedHealth;
        healthText.text = $"{LevelManager.Instance.planetHealth.currentHealth:##0}";
        highscoreText.text = $"Highscore: {Mathf.Max(LevelManager.Instance.score, LevelManager.Instance.GetHighScore()):###,##0}";
        scoreText.text = $"Score: {LevelManager.Instance.score:###,##0}";*/
    }

    void IndicateDamage()
    {
        damageIndicatorImage.DOKill();

        var tween = damageIndicatorImage.DOFade(damageIndicatorMaxOpacity, damageIndicatorAnimationDuration);
        tween.OnComplete(() =>
        {
            damageIndicatorImage.DOKill();
            damageIndicatorImage.DOFade(0, damageIndicatorAnimationDuration).Play();
        });

        tween.Play();
    }
}
