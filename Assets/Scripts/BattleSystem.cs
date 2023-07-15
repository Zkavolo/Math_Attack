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
    public Unit playerUnit;
    public Unit enemyUnit;

    [Header("Text")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI mathText;
    public TextMeshProUGUI TimerText;

    [Header("Unit HUD")]
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [Header("Win Lose UI")]
    public WinUI WUI;
    public LoseUI LUI;

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

        if(Input.anyKeyDown){
            input.ActivateInputField();
        }
    }

    public IEnumerator SetupBattle()
    {
        dialogueText.text = "The " + enemyUnit.unitName + " has appeared ";

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

    //After battle condition

    public void EndBattle()
    {
        if(state == BattleState.WON){
            dialogueText.text = "You won the battle!";
            WUI.WinCondition();
        } else if(state == BattleState.LOST){
            dialogueText.text = "You have been defeated";
            LUI.LoseCondition();
        }
    }

    //random math question generator

    public void MathQuestions()
    {
        QuickTimeAction.SetActive(true);
        input.text = "";
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
        //Player Ult
        if(userAction == 1){
            if(answer == resOper){
                StartCoroutine(playerAct.PlayerHeavyAttack());
                userAction = 1;
            } else if (answer != resOper){
                StartCoroutine(playerAct.PlayerFail());
                userAction = 0;
            }
        }
        //Player Normal Attack
        else if(userAction == 2){
            if(answer == resOper){
                StartCoroutine(playerAct.PlayerNormalAttack());
                userAction = 2;
            } else if (answer != resOper){
                StartCoroutine(playerAct.PlayerFail());
                userAction = 0;
            }
        }
        //Player Block
        else if(userAction == 3){
            if(answer == resOper){
                StartCoroutine(playerAct.PlayerBlock());
                userAction = 3;
            } else if (answer != resOper){
                StartCoroutine(playerAct.PlayerFail());
                userAction = 0;
            }
        }
        //Player Heal
        else if(userAction == 4){
            if(answer == resOper){
                StartCoroutine(playerAct.PlayerHeal());
                userAction = 4;
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
