using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinUI : MonoBehaviour
{
    public GameObject WinPanel;

    public void WinCondition(){
        Time.timeScale = 0f;
        WinPanel.SetActive(true);
    } 
}
