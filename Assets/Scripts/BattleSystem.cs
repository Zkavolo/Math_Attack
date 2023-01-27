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
    public TextMeshProUGUI TimerText;

    [Header("Unit HUD")]
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [Header("Action HUD")]
    public GameObject QuickTimeAction;

    [Header("State of game")]
    public BattleState state;
    public PlayerAction playerAct;

    [Header("Input field")]
    public TMP_InputField input;

    [Header("Private variables")]
    public string answer;
    public string resOper;
    public bool validation;
    public int userAction;
    public float Timer;
    public bool TimerRun;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    void Update()
    {
        if(TimerRun)
        {
            if(Timer > 0)
            {
                Timer -= Time.deltaTime;
                DisplayTimer(Timer);
            } else {
                TimerRun = false;
                StartCoroutine(playerAct.PlayerFail());
                userAction = 0;
            }
        }
    }

    public IEnumerator SetupBattle()
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
        playerAct.PlayerTurn();
    }

    public void StateWon(){
        state = BattleState.WON;
    }

    public void StateLost(){
        state = BattleState.LOST;
    }

    public void StatePlayerTurn(){
        state = BattleState.PLAYERTURN;
    }

    public void StateEnemyTurn(){
        state = BattleState.ENEMYTURN;
    }

    //Enemy Turn

    // public IEnumerator EnemyTurn()
    // {
    //     dialogueText.text = enemyUnit.unitName + "does an action";

    //     yield return new WaitForSeconds(1f);

    //     bool isdead = playerUnit.TakeDamage(enemyUnit.damage);
        
    //     playerHUD.SetHP(playerUnit.currentHP);

    //     yield return new WaitForSeconds(1f);

    //     if(isdead){
    //         StateLost();
    //         EndBattle();
    //     } else {
    //         state = BattleState.PLAYERTURN;
    //         playerAct.PlayerTurn();
    //     }
    // }

    //After battle condition

    public void EndBattle()
    {
        if(state == BattleState.WON){
            dialogueText.text = "You won the battle!";
        } else if(state == BattleState.LOST){
            dialogueText.text = "You have been defeated";
        }
    }

    //random math question generator

    public void MathQuestions()
    {
        QuickTimeAction.SetActive(true);
        playerAct.playerAction.SetActive(false);
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

    //Timer

    public void QTimer()
    {
        TimerRun = true;
    }

    public void DisplayTimer(float CurTime)
    {
        CurTime += 1;
        int DisplayTime = Mathf.FloorToInt(CurTime);
        TimerText.text = string.Format("{00}",DisplayTime);
    }

    public void ResetTimer(){
        Timer = 10;
    }

    //validasi jawaban

    public void ValidateAns()
    {
        //Player Heavy Attack
        if(userAction == 1){
            if(answer == resOper){
                StartCoroutine(playerAct.PlayerHeavyAttack());
                userAction = 0;
            } else if (answer != resOper){
                StartCoroutine(playerAct.PlayerFail());
                userAction = 0;
            }
        }
        //Player Normal Attack
        else if(userAction == 2){
            if(answer == resOper){
                StartCoroutine(playerAct.PlayerNormalAttack());
                userAction = 0;
            } else if (answer != resOper){
                StartCoroutine(playerAct.PlayerFail());
                userAction = 0;
            }
        }
        //Player Block
        else if(userAction == 3){
            if(answer == resOper){
                StartCoroutine(playerAct.PlayerBlock());
                userAction = 0;
            } else if (answer != resOper){
                StartCoroutine(playerAct.PlayerFail());
                userAction = 0;
            }
        }
        //Player Heal
        else if(userAction == 4){
            if(answer == resOper){
                StartCoroutine(playerAct.PlayerHeal());
                userAction = 0;
            } else if (answer != resOper){
                StartCoroutine(playerAct.PlayerFail());
                userAction = 0;
            }
        }
    }

    //determine set answer

    public void SetAnswer()
    {
        TimerRun = false;
        answer = input.text;
        QuickTimeAction.SetActive(false);
        ValidateAns();
        ResetTimer();
    }
}
