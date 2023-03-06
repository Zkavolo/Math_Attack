using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ChangeScene : MonoBehaviour
{
    public int scenecode;

    void Update()
    {
        //Paise Condition
        // if (GamePaused){
        //     UI.SetActive(true);
        // } else {
        //     UI.SetActive(false);
        // }
        // //Win or Lose Condition
        // if (BattleSystem.state == BattleState.WON){
        //     WinCondition();
        // } else if (BattleSystem.state == BattleState.LOST){

        // }
    }
    public void ChangeTo()
    {
        SceneManager.LoadScene(scenecode);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
