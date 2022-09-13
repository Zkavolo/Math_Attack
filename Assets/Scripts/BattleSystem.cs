using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    [Header("Game Object")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    [Header("Game Object Position")]
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    [Header("Unit Variable")]
    Unit playerUnit;
    Unit enemyUnit;

    [Header("Text")]
    public TextMeshProUGUI dialogueText;

    [Header("Unit HUD")]
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [Header("Action HUD")]
    public GameObject playerAction;

    [Header("State of game")]
    public BattleState state;


    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject PlayerGo = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = PlayerGo.GetComponent<Unit>();

        GameObject EnemyGo = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = EnemyGo.GetComponent<Unit>();

        dialogueText.text = "An " + enemyUnit.unitName + " appeared ";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(3f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    //Player Turn

    void PlayerTurn()
    {
        dialogueText.text = "What will you do ?";
        playerAction.SetActive(true);
    }

    public void HeavyAtkButton()
    {
        if(state != BattleState.PLAYERTURN)
            return ;

        playerAction.SetActive(false);
        StartCoroutine(PlayerHeavyAttack());    
    }

    IEnumerator PlayerHeavyAttack()
    {
        //Generate Random Number

        //add if function
        bool isdead = enemyUnit.TakeDamage(playerUnit.damage);
        
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful";

        yield return new WaitForSeconds(2f);

        if(isdead){
            state = BattleState.WON;
            EndBattle();
        } else {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    //Enemy Turn

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + "does an action";

        yield return new WaitForSeconds(1f);

        bool isdead = playerUnit.TakeDamage(enemyUnit.damage);
        
        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if(isdead){
            state = BattleState.LOST;
            EndBattle();
        } else {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON){
            dialogueText.text = "You won the battle!";
        } else if(state == BattleState.LOST){
            dialogueText.text = "You have been defeated";
        }
    }
}
