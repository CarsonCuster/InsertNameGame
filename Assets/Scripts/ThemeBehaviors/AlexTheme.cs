using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlexTheme : ThemeSpecificContent
{
    public int amountOfHealthBuys = 3;
    private int healthBuysOnStart;
    public GameObject healthBuysUI;
    public TMP_Text amountOfBuys;
    public TMP_Text buyAvailable;
    private int bulkBuyCount = 0;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        amountOfBuys.text = $"{amountOfHealthBuys}";
        healthBuysOnStart = amountOfHealthBuys;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UpdateThings()
    {
        base.UpdateThings();
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(zachGameLogic.playerScore >= 5 && amountOfHealthBuys > 0 && bulkBuyCount != 0)
            {
                bulkBuyCount = 0;
                int healthToAdd = (zachGameLogic.playerScore - zachGameLogic.playerScore % 5) / 5;
                zachGameLogic.AddHealth(healthToAdd);
                zachGameLogic.playerScore = zachGameLogic.playerScore % 5;
                zachGameLogic.scoreText.text = $"{zachGameLogic.playerScore}";
                buyAvailable.gameObject.SetActive(false);
                amountOfHealthBuys--;
                amountOfBuys.text = $"{amountOfHealthBuys}";
            //healthBuysText.text = $"{amountOfHealthBuys}";
            }
        }
    }

    public override void OnScoreIncrease()
    {
        base.OnScoreIncrease();
        if(zachGameLogic.playerScore % 5 == 0)
        {
            if(!buyAvailable.gameObject.activeInHierarchy) buyAvailable.gameObject.SetActive(true);
            bulkBuyCount++;
            buyAvailable.text = $"Buy {bulkBuyCount} Health";
        }
    }

    public override void StartGame()
    {
        base.StartGame();
        healthBuysUI.SetActive(true);
    }

    public override void QuitToMain()
    {
        base.QuitToMain();
        healthBuysUI.SetActive(false);
    }
    public override void Restart()
    {
        base.Restart();
        buyAvailable.gameObject.SetActive(false);
        amountOfHealthBuys = healthBuysOnStart;
        amountOfBuys.text = $"{amountOfHealthBuys}";
    }
    public override void OnGameOver()
    {
        base.OnGameOver();
    }
    public override void OnThemeEnter()
    {
        base.OnThemeEnter();
    }
}
