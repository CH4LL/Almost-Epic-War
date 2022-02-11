using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManaDisplay : MonoBehaviour
{
    [SerializeField] private GameManager gameManager = default;
    [SerializeField] private TextMeshProUGUI uiText = default;

    void Start()
    {
        UpdateMana();
    }

    public void UpdateMana()
    {
        uiText.text = "Mana: " + Mathf.RoundToInt(gameManager.currentMana) + "/" + gameManager.manaPool;
    }
}

