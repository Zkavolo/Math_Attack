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
    public float Timer;
    public bool TimerRun = false;

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
                StartCoroutine(PlayerFail());
                userAction = 0;
            }
        }
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
        playerUnit.NotBlocking();
        playerAction.SetActive(true);
        QuickTimeAction.SetActive(false);
        Timer = 10;
    }

    // Player action Buttons

    public void HeavyAtkButton()
    {
        if(state != BattleState.PLAYERTURN)
            return ;

        userAction = 1;
        MathQuestions();
        QTimer();    
    }

    public void NormalAtkButton()
    {
        if(state != BattleState.PLAYERTURN)
            return ;

        userAction = 2;
        MathQuestions();
        QTimer();    
    }

    public void BlockButton()
    {
        if(state != BattleState.PLAYERTURN)
            return ;

        userAction = 3;
        MathQuestions();
        QTimer();    
    }

    public void HealButton()
    {
        if(state != BattleState.PLAYERTURN)
            return ;

        userAction = 4;
        MathQuestions();
        QTimer();    
    }

    //Player Actions

    IEnumerator PlayerHeavyAttack()
    {
        bool isdead = enemyUnit.TakeDamage(playerUnit.damage);
        
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The heavy attack is successful";

        yield return new WaitForSeconds(2f);

        if(isdead){
            state = BattleState.WON;
            EndBattle();
        } else {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerNormalAttack()
    {
        bool isdead = enemyUnit.TakeDamage(playerUnit.damage/2);
        
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

    IEnumerator PlayerBlock()
    {
        playerUnit.Blocking();

        dialogueText.text = "Player is blocking";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(playerUnit.damage);

        dialogueText.text = "Player is healing";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerFail()
    {
        dialogueText.text = "Player failed to do an action";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
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

    //After battle condition

    void EndBattle()
    {
        if(state == BattleState.WON){
            dialogueText.text = "You won the battle!";
        } else if(state == BattleState.LOST){
            dialogueText.text = "You have been defeated";
        }
    }

    //random math question generator

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

    //Timer

    void QTimer()
    {
        TimerRun = true;
    }

    void DisplayTimer(float CurTime)
    {
        CurTime += 1;
        int DisplayTime = Mathf.FloorToInt(CurTime);
        TimerText.text = string.Format("{00}",DisplayTime);
    }

    //validasi jawaban

    void ValidateAns()
    {
        //Player Heavy Attack
        if(userAction == 1){
            if(answer == resOper){
                StartCoroutine(PlayerHeavyAttack());
                userAction = 0;
            } else if (answer != resOper){
                StartCoroutine(PlayerFail());
                userAction = 0;
            }
        }
        //Player Normal Attack
        else if(userAction == 2){
            if(answer == resOper){
                StartCoroutine(PlayerNormalAttack());
                userAction = 0;
            } else if (answer != resOper){
                StartCoroutine(PlayerFail());
                userAction = 0;
            }
        }
        //Player Block
        else if(userAction == 3){
            if(answer == resOper){
                StartCoroutine(PlayerBlock());
                userAction = 0;
            } else if (answer != resOper){
                StartCoroutine(PlayerFail());
                userAction = 0;
            }
        }
        //Player Heal
        else if(userAction == 4){
            if(answer == resOper){
                StartCoroutine(PlayerHeal());
                userAction = 0;
            } else if (answer != resOper){
                StartCoroutine(PlayerFail());
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
    }
}
