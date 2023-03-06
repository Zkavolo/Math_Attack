using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinUI : MonoBehaviour
{
    public GameObject WinPanel;
    // public BattleState curstate;
    // public BattleSystem BattleSystem;


    // void update()
    // {
    //     if(curstate != BattleSystem.state){
    //         curstate = BattleSystem.state;
    //     } else if(curstate == BattleState.WON){
    //         WinCondition();
    //         Debug.Log("it works");
    //     }
    // }

    public void WinCondition(){
        Time.timeScale = 0f;
        WinPanel.SetActive(true);
    } 
}
