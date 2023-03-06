using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseUI : MonoBehaviour
{
    public GameObject UI;

    public void Resume(){
        Time.timeScale = 1f;
        UI.SetActive(false);
    }

    public void Pause(){
        Time.timeScale = 0f;
        UI.SetActive(true);
    }
}
