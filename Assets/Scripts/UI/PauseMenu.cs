using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string mainMenuName;
    public GameObject rootPanel;

    public GameObject pausePanel;
    public GameObject controlsPanel;

    private void Pause()
    {
        Time.timeScale = 0;
        HelperUtilities.UpdateCursorLock(false);

        pausePanel.SetActive(true);
        pausePanel.GetComponent<MenuPage>().Show();
    }

    public void UnPause()
    {
        Time.timeScale = 1;

        if (controlsPanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
            pausePanel.GetComponent<MenuPage>().Show();
            controlsPanel.SetActive(false);
            controlsPanel.GetComponent<MenuPage>().Hide();
        }
        
        rootPanel.gameObject.SetActive(false);
        HelperUtilities.UpdateCursorLock(true);
    }

    public void GoToMainMenu()
    {
        ScreenFader.Instance.FadeOut(-1, () => { SceneManager.LoadScene(mainMenuName); });
    }

    void OnPause(InputValue inputValue)
    {
        if (rootPanel.gameObject.activeInHierarchy)
        {
            UnPause();
            rootPanel.gameObject.SetActive(false);
        }
        else
        {
            Pause();
            rootPanel.gameObject.SetActive(true);
        }
    }
}
