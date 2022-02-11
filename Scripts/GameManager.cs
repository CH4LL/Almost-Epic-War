using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // UI elements
    [SerializeField] private ManaDisplay manaDisplay = default;
    [SerializeField] private VictoryText victoryText = default;
    [SerializeField] private CastleHealth allyCastleUI = default;
    [SerializeField] private CastleHealth enemyCastleUI = default;
    // Difficulty Buttons
    [SerializeField] private GameObject easyButton = default;
    [SerializeField] private GameObject mediumButton = default;
    [SerializeField] private GameObject hardButton = default;

    // player and enemy castle
    [SerializeField] public UnitController playerCastle = default;
    [SerializeField] public UnitController enemyCastle = default;

    // unit types
    [SerializeField] private GameObject enemyFootman = default;
    [SerializeField] private GameObject enemyArcher = default;
    [SerializeField] private GameObject enemyMage = default;
    [SerializeField] private GameObject allyFootman = default;
    [SerializeField] private GameObject allyArcher = default;
    [SerializeField] private GameObject allyMage = default;

    // variables used for defining strengths of player objects in the game
    public int footmanLevel = 1, archerLevel = 1, mageLevel = 1, castleCrossbowLevel = 1;

    // player resources
    public int manaPool = 200;
    public float currentMana = 0;

    // other variables
    public double difficultyScaling = default;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            victoryText.GameOver(3);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(currentMana >= manaPool/2)
            {
                UpgradeMana();
            }
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(currentMana >= 15)
            {
                currentMana -= 15;
                Instantiate(allyFootman);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentMana >= 20)
            {
                currentMana -= 20;
                Instantiate(allyArcher);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentMana >= 25)
            {
                currentMana -= 25;
                Instantiate(allyMage);
            }
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            if(currentMana >= castleCrossbowLevel * 300 && castleCrossbowLevel < 7)
            {
                currentMana -= castleCrossbowLevel * 300;
                castleCrossbowLevel++;
            }
        }
    }

    // Constant gain of mana based on various variables started at the beggining of the game
    IEnumerator GainMana()
    {
        float basicGain;
        float randomFactor;
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (currentMana < manaPool)
            {
                basicGain = 0.2f + (float)manaPool / 1000;
                randomFactor = Random.Range(0.0001f, 0.0007f)*currentMana;
                currentMana += basicGain + randomFactor;
            }
            if(currentMana > manaPool)
            {
                currentMana = manaPool;
            }
            manaDisplay.UpdateMana();
            // Debug.Log("current mana: " + Mathf.RoundToInt(currentMana));
        }
    }

    void UpgradeMana()
    {
        currentMana -= manaPool / 2;
        manaPool = Mathf.RoundToInt(1.3f * manaPool);
        manaDisplay.UpdateMana();
    }

    IEnumerator CheckGameState()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            enemyCastleUI.UpdateHealth();
            allyCastleUI.UpdateHealth();
            if (playerCastle.health <= 0 )
            {
                // game over = loss
                victoryText.GameOver(2);
            }
            if (enemyCastle.health <= 0)
            {
                // game over = victory
                victoryText.GameOver(1);
            }
        }
    }

    //enemy unit spawn logic
    IEnumerator RecruitEnemyUnit()
    {
        double spawnDelay;
        int unitType;
        while(true)
        {
            spawnDelay = Random.Range(2f, 5f) * (1d - difficultyScaling / 100);
            yield return new WaitForSeconds((float)spawnDelay);
            unitType = Random.Range(1, 4);
            switch(unitType)
            {
                case 1:
                    Instantiate(enemyFootman);
                    break;
                case 2:
                    Instantiate(enemyArcher);
                    break;
                case 3:
                    Instantiate(enemyMage);
                    break;
                default:
                    break;
            }
            if(difficultyScaling<75)
            {
                difficultyScaling += 0.1f;
            }
        }
    }

    public void SetDifficulty(int difficulty)
    {
        difficultyScaling = difficulty;
        easyButton.SetActive(false);
        mediumButton.SetActive(false);
        hardButton.SetActive(false);

        Time.timeScale = 1;
        StartCoroutine(GainMana());
        StartCoroutine(CheckGameState());
        StartCoroutine(RecruitEnemyUnit());
    }

    // Deprecated
    /*
    public int ReturnIntMemberVariable(int x)
    {
        switch(x)
        {
            case 1:
                return this.footmanLevel;
            case 2:
                return this.archerLevel;
            case 3:
                return this.mageLevel;
            case 4:
                return this.castleCrossbowLevel;
            default:
                return 0;
        }
    }

    public float ReturnFloatMemberVariable(int x)
    {
        switch (x)
        {
            case 1:
                return this.manaPool;
            case 2:
                return this.castleHealth;
            default:
                return 0;
        }
    }
    */
}
