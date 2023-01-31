using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyAI_1 : MonoBehaviour
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
    
    //Enemy Turn

    public IEnumerator EnemyTurn()
    {
        enemyUnit.NotBlocking();

        dialogueText.text = enemyUnit.unitName + " Turn";

        yield return new WaitForSeconds(2f);

        if(BattleSystem.userAction==1){

        dialogueText.text = enemyUnit.unitName + " retaliates";

        yield return new WaitForSeconds(2f);

        bool isdead = playerUnit.TakeDamage(enemyUnit.damage*2);
        playerHUD.SetHP(playerUnit.currentHP);

            if(isdead){
                BattleSystem.StateLost();
                BattleSystem.EndBattle();
            } else {
                BattleSystem.StatePlayerTurn();
                playerAct.PlayerTurn();
            }

        } else if(BattleSystem.userAction==2) {

        dialogueText.text = enemyUnit.unitName + " attacks";

        yield return new WaitForSeconds(2f);

        bool isdead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);

            if(isdead){
                BattleSystem.StateLost();
                BattleSystem.EndBattle();
            } else {
                BattleSystem.StatePlayerTurn();
                playerAct.PlayerTurn();
            }

        } else {

        enemyUnit.Blocking();

        dialogueText.text = enemyUnit.unitName + " blocks";

        yield return new WaitForSeconds(2f);
        
        BattleSystem.StatePlayerTurn();
        playerAct.PlayerTurn();
        }
    }
}