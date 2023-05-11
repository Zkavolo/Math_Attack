using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoseUI : MonoBehaviour
{
    public GameObject LosePanel;

    public void LoseCondition()
    {
        Time.timeScale = 0f;
        LosePanel.SetActive(true);
    }
}
