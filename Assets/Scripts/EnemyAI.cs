using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    [Header("Unit Variable")]
    public Unit playerUnit;
    public Unit enemyUnit;

    [Header("Text")]
    public TextMeshProUGUI dialogueText;

    [Header("Unit HUD")]
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [Header("Script Link")]
    public BattleSystem BattleSystem;
    public PlayerAction playerAct;
    public TreeNode tn;

    [Header("Unit only variables")]
    public Animator Enemyanim;
    public int UltCharge;
    public int enemyAction;

    //Enemy Turn

    public IEnumerator EnemyTurn()
    {
        enemyUnit.NotBlocking();
        
        yield return new WaitForSeconds(2f);

        //make a new treenode using current unitstats
        tn = new TreeNode(new UnitStats(BattleSystem.userAction, enemyAction, playerUnit.currentHP, enemyUnit.currentHP, playerAct.UltCharge, UltCharge, playerUnit.HealCharge, enemyUnit.HealCharge, playerUnit.damage, enemyUnit.damage, playerUnit.shielded, enemyUnit.shielded)); //create a new TreeNode

        //begin MCTS
        tn.iterateMCTS();

        Debug.Log("children count = "+tn.children.Count);
        foreach(TreeNode child in tn.children){
            Debug.Log(child.unitstats.enemyAction+" = "+child.uctValue+" "+child.totValue+" "+child.nVisits);
        }

        tn = tn.select();

        // Debug.Log("enemy action = "+tn.unitstats.enemyAction);

        int pickedAct = tn.unitstats.enemyAction;
        

        if(pickedAct == 1){
            StartCoroutine(EnemyRetaliates());
        } else if(pickedAct == 2){
            StartCoroutine(EnemyAttacks());
        } else if(pickedAct == 3){
            StartCoroutine(EnemyBlocking());
        } else if(pickedAct == 4){
            StartCoroutine(EnemyHeal());
        }
    }

    public IEnumerator EnemyRetaliates()
    {
        Enemyanim.SetTrigger("EnemyUlt");

        yield return new WaitForSeconds(3f);

        bool isdead = playerUnit.TakeDamage(enemyUnit.damage*2);
        playerHUD.SetHP(playerUnit.currentHP);

        dialogueText.text = enemyUnit.unitName + " retaliates";

        UltCharge = 0;

        yield return new WaitForSeconds(2f);

        if(isdead){
            BattleSystem.StateLost();
            BattleSystem.EndBattle();
        } else {
            BattleSystem.StatePlayerTurn();
            playerAct.PlayerTurn();
        }
    }

    public IEnumerator EnemyAttacks()
    {
        Enemyanim.SetTrigger("EnemyAtk");

        yield return new WaitForSeconds(0.45f);

        bool isdead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);

        dialogueText.text = enemyUnit.unitName + " attacks";

        if(UltCharge>=0&&UltCharge<3){
            UltCharge++;
        }

        yield return new WaitForSeconds(2f);

        if(isdead){
            BattleSystem.StateLost();
            BattleSystem.EndBattle();
        } else {
            BattleSystem.StatePlayerTurn();
            playerAct.PlayerTurn();
        }
    }

    public IEnumerator EnemyBlocking()
    {
        enemyUnit.Blocking();

        dialogueText.text = enemyUnit.unitName + " blocks";

        yield return new WaitForSeconds(2f);
        
        BattleSystem.StatePlayerTurn();
        playerAct.PlayerTurn();
    }

    public IEnumerator EnemyHeal()
    {
        enemyUnit.Heal(enemyUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = enemyUnit.unitName + " is healing";
        enemyUnit.HealCharge--;

        yield return new WaitForSeconds(2f);

        BattleSystem.StatePlayerTurn();
        playerAct.PlayerTurn();
    }
}

