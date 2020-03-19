using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{
    public Toggle invertY;

    void OnEnable()
    {
        invertY.isOn = GameManager.Instance.GetGameData().playerSettings.shouldInvertY;
    }

    public void UpdateInvertY()
    {
        GameManager.Instance.gameData.playerSettings.shouldInvertY = invertY.isOn;

        GameManager.Instance.SaveGameData();
    }
}
