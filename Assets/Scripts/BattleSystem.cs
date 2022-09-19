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
    public TextMeshProUGUI mathText;

    [Header("Unit HUD")]
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [Header("Action HUD")]
    public GameObject playerAction;
    public GameObject QuickTimeAction;

    [Header("State of game")]
    public BattleState state;

    [Header("Input field")]
    public TMP_InputField input;

    [Header("Private variables")]
    public string answer;
    public string resOper;
    public bool validation = false;
    public int userAction;

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
        QuickTimeAction.SetActive(false);
    }

    public void HeavyAtkButton()
    {
        if(state != BattleState.PLAYERTURN)
            return ;

        userAction = 1;
        MathQuestions();    
    }

    IEnumerator PlayerHeavyAttack()
    {
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

    void MathQuestions()
    {
        QuickTimeAction.SetActive(true);
        playerAction.SetActive(false);
        int firstVar = Random.Range(0, 100);
        int SecondVar = Random.Range(0, 100);
        int iniOper = Random.Range(0, 2);
        char mathOper;
        int totalOper;

            if(iniOper == 0){
                mathOper = '+';
                totalOper = firstVar + SecondVar;
                resOper = totalOper.ToString();
            } else if(iniOper == 1) {
                mathOper = '-';
                totalOper = firstVar - SecondVar;
                resOper = totalOper.ToString();
            } else {
                mathOper = '*';
                totalOper = firstVar * SecondVar;
                resOper = totalOper.ToString();
            }

        mathText.text = firstVar + " " + mathOper + " " + SecondVar + " = ??? ";
    }

    void ValidateAns()
    {
        //Player Heavy Attack
        if(userAction == 1){
            if(answer == resOper){
                StartCoroutine(PlayerHeavyAttack());
                userAction = 0;
            } else if (answer != resOper){
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
                userAction = 0;
            }
        }    
    }

    public void SetAnswer()
    {
        answer = input.text;
        QuickTimeAction.SetActive(false);
        ValidateAns();
    }
}
