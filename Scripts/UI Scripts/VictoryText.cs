using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText = default;

    public void GameOver(int state)
    {
        switch(state)
        {
            case 1:
                Time.timeScale = 0;
                uiText.text = "Victory!";
                break;
            case 2:
                Time.timeScale = 0;
                uiText.text = "Defeat!";
                break;
            case 3:
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                    uiText.text = "";
                }
                else
                {
                    Time.timeScale = 0;
                    uiText.text = "Paused!";
                }
                break;
        }
    }
}

