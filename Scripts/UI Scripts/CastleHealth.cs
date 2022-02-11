using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CastleHealth : MonoBehaviour
{
    [SerializeField] private GameManager gameManager = default;
    [SerializeField] private TextMeshProUGUI uiText = default;
    public int isAlly;

    void Start()
    {
        UpdateHealth();
    }


   public void UpdateHealth()
    {
        if(isAlly == 1)
            uiText.text = gameManager.playerCastle.health + "/10000";
        else
            uiText.text = gameManager.enemyCastle.health + "/10000";
    }
}
