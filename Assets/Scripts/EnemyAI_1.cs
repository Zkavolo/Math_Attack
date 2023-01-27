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
        dialogueText.text = enemyUnit.unitName + "attacks";

        yield return new WaitForSeconds(1f);

        bool isdead = playerUnit.TakeDamage(enemyUnit.damage);
        
        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if(isdead){
            BattleSystem.StateLost();
            BattleSystem.EndBattle();
        } else {
            BattleSystem.StatePlayerTurn();
            playerAct.PlayerTurn();
        }
    }
}
