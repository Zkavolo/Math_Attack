using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAction : MonoBehaviour
{
    [Header("Unit Variable")]
    public Unit playerUnit;
    public Unit enemyUnit;

    [Header("Text")]
    public TextMeshProUGUI dialogueText;

    [Header("Unit HUD")]
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [Header("Action HUD")]
    public GameObject playerAction;

    [Header("Buttons")]
    public Button UltButton;
    public Button HealChargeButton;

    [Header("Script Link")]
    public BattleSystem BattleSystem;
    public EnemyAI EnemyAi;

    [Header("Unit only variables")]
    public int UltCharge;
    public Animator Playeranim;


    //Player Turn
    public void PlayerTurn()
    {
        dialogueText.text = "What will you do ?";
        playerUnit.NotBlocking();
        playerAction.SetActive(true);
        if(UltCharge != 3){
            UltButton.interactable = false;
        } else if (UltCharge == 3) {
            UltButton.interactable = true;
        }

        if(playerUnit.HealCharge != 0){
            HealChargeButton.interactable = true;
        } else {
            HealChargeButton.interactable = false;
        }
        BattleSystem.QuickTimeAction.SetActive(false);
        BattleSystem.Timer = 10;
    }

    // Player action Buttons
    public void HeavyAtkButton()
    {
        BattleSystem.userAction = 1;
        BattleSystem.MathQuestions();
        BattleSystem.QTimer();  
    }

    public void NormalAtkButton()
    {
        BattleSystem.userAction = 2;
        BattleSystem.MathQuestions();
        BattleSystem.QTimer();    
    }

    public void BlockButton()
    {
        BattleSystem.userAction = 3;
        BattleSystem.MathQuestions();
        BattleSystem.QTimer();    
    }

    public void HealButton()
    {
        BattleSystem.userAction = 4;
        BattleSystem.MathQuestions();
        BattleSystem.QTimer();    
    }

    //Player Actions
    public IEnumerator PlayerHeavyAttack()
    {   
        Playeranim.SetTrigger("PlayerUlt");

        yield return new WaitForSeconds(2f);

        bool isdead = enemyUnit.TakeDamage(playerUnit.damage*2);
        
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The Elemental Burst landed";

        UltCharge = 0;

        yield return new WaitForSeconds(2f);

        if(isdead){
            BattleSystem.StateWon();
            BattleSystem.EndBattle();
        } else {
            BattleSystem.StateEnemyTurn();
            StartCoroutine(EnemyAi.EnemyTurn());
        }
    }

    public IEnumerator PlayerNormalAttack()
    {
        Playeranim.SetTrigger("PlayerAtk");

        yield return new WaitForSeconds(0.30f);

        bool isdead = enemyUnit.TakeDamage(playerUnit.damage);
        
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful";

        if(UltCharge>=0&&UltCharge<3){
            UltCharge++;
        }

        yield return new WaitForSeconds(2f);

       if(isdead){
            BattleSystem.StateWon();
            BattleSystem.EndBattle();
        } else {
            BattleSystem.StateEnemyTurn();
            StartCoroutine(EnemyAi.EnemyTurn());
        }
    }

    public IEnumerator PlayerBlock()
    {
        playerUnit.Blocking();

        dialogueText.text = "Player is blocking";

        yield return new WaitForSeconds(2f);

        BattleSystem.StateEnemyTurn();
        StartCoroutine(EnemyAi.EnemyTurn());
    }

    public IEnumerator PlayerHeal()
    {
        playerUnit.Heal(playerUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "Player is healing";
        playerUnit.HealCharge--;

        yield return new WaitForSeconds(2f);

        BattleSystem.StateEnemyTurn();
        StartCoroutine(EnemyAi.EnemyTurn());
    }

    public IEnumerator PlayerFail()
    {
        dialogueText.text = "Player failed to do an action";

        yield return new WaitForSeconds(2f);

        BattleSystem.StateEnemyTurn();
        StartCoroutine(EnemyAi.EnemyTurn());
    }
}
