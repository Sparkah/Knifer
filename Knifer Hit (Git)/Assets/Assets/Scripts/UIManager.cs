using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public ArrowBehaviour arrowBehaviour;
    private SaveData saveData;

    public Canvas canvasMenu;
    public Canvas InGameMenu;
    public Canvas GameOver;

    [SerializeField]
    private Text InGameArrowCounter;

    [SerializeField]
    private Text Level;

    private int totalArrowNumber;
    public int numberOfArrowsToWin;

    public Text HighScore;

    private GameObject saveScorer;

    public GameObject LevelConmpleteMenu;
    public bool firstShot = false;

    public Text appleAmountMainMenu;
    public Text appleAmountNextLevel;
    public Text appleAmountLost;

    private void Start()
    {
        saveScorer = GameObject.FindGameObjectWithTag("ScoreSaver");
        saveData = saveScorer.GetComponent<SaveData>();

        Time.timeScale = 0;
        totalArrowNumber = numberOfArrowsToWin + saveData.maxLevel;
        saveData.FindGameObject();

        saveData.HighestScoreSaver();
        InGameArrowCounter.text = "Осталось кинжалов: " + (numberOfArrowsToWin + saveData.maxLevel).ToString() + " / " + totalArrowNumber.ToString();
    }

    private void Update()
    {
        if (firstShot == true)
        {
            InGameArrowCounter.text = "Осталось кинжалов: " + numberOfArrowsToWin.ToString() + " / " + totalArrowNumber.ToString();
        }
        Level.text = "Уровень " + saveData.maxLevel;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        canvasMenu.enabled = false;
    }

    public void NextLevel()
    {
        saveData.nextLevel = true;
        saveData.menu = false;
        saveData.SaveScore();
        SceneManager.LoadScene("MainScene");
    }

    public void MainMenu()
    {
        canvasMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
        saveData.menu = true;
        SceneManager.LoadScene("MainScene");
    }
}