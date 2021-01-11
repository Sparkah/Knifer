using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    public int maxLevel = 1;
    private int highScore;
    private UIManager uIManager;
    private GameObject UIData;
    private ArrowBehaviour arrowBehaviour;
    private GameObject arrow;

    public bool nextLevel;
    public bool gameOver;
    public bool menu;

    private int currentAppleAmount;
    public int highScoreNew = 0;
    private int scoreSaverBeforeQuit;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("Score");
        currentAppleAmount = PlayerPrefs.GetInt("Apple");
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainScene");
    }

    public void FindGameObject()
    {
        UIData = GameObject.FindGameObjectWithTag("UI");
        uIManager = UIData.GetComponent<UIManager>();
        if (nextLevel == true && gameOver == false)
        {
            if (menu == false)
            {
                uIManager.canvasMenu.gameObject.SetActive(false);
                uIManager.InGameMenu.gameObject.SetActive(true);
                Time.timeScale = 1;
            }
        }

        arrow = GameObject.FindGameObjectWithTag("Arrow");
        arrowBehaviour = arrow.GetComponent<ArrowBehaviour>();
        arrowBehaviour.numberOfArrowsToWin += maxLevel;
        HighestScoreSaver();
    }

    public void SaveScore()
    {
        if (highScoreNew>highScore)
        {
            scoreSaverBeforeQuit = highScoreNew;
            PlayerPrefs.SetInt("Score", highScoreNew);
            PlayerPrefs.Save();
            HighestScoreSaver();
        }
    }

    public void HighestScoreSaver()
    {
        if (highScoreNew > highScore)
        {
            uIManager.HighScore.text = "Максимальный уровень: " + highScoreNew.ToString();
        }
        if (highScore > highScoreNew)
        {
            uIManager.HighScore.text = "Максимальный уровень: " + highScore.ToString();
        }
        if (scoreSaverBeforeQuit > highScore && scoreSaverBeforeQuit>highScoreNew)
        {
            uIManager.HighScore.text = "Максимальный уровень: " + scoreSaverBeforeQuit.ToString();
        }
        DisplayApple();
    }

    public void DisplayApple()
    {
        uIManager.appleAmountMainMenu.text = "Всего яблок: " + currentAppleAmount.ToString();
        uIManager.appleAmountLost.text = "Всего яблок: " + currentAppleAmount.ToString();
        uIManager.appleAmountNextLevel.text = "Всего яблок: " + currentAppleAmount.ToString();
    }

    public void ReadApple()
    {
        PlayerPrefs.SetInt("Apple", currentAppleAmount+=1);
        PlayerPrefs.Save();
        DisplayApple();
    }
}