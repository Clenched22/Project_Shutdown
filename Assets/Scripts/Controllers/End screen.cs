using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public bool Win;
    public string TimeTookText;
    [SerializeField] TMP_Text WinLoseText;
    [SerializeField] TMP_Text EndScreenTimerText;
    [SerializeField] string WinText;
    [SerializeField] GameObject LoseText;
    [SerializeField] GameObject WinImage;
    [SerializeField] GameObject LoseImage;
    [SerializeField] GameObject timeItTookText;

    private void Start()
    {
        Win = FindObjectOfType<LevelController>().Win;
        if (Win)
        {
            WinLoseText.text = WinText;
            WinImage.SetActive(true);
            LoseImage.SetActive(false);
            timeItTookText.SetActive(true);
        }
        else
        {
            //LoseText.SetActive(true);
            WinImage.SetActive(false);
            LoseImage.SetActive(true);
            timeItTookText.SetActive(false);
        }
        float timeToShow = FindObjectOfType<LevelController>().TimerAmount;
        timeToShow -= FindObjectOfType<LevelController>().CurrentTime;
        Debug.Log(timeToShow);
        TimeSpan time = TimeSpan.FromSeconds(timeToShow);
        EndScreenTimerText.enabled = true;
    }

    void Update()
    {
        float timeToShow = FindObjectOfType<LevelController>().TimerAmount;
        timeToShow -= FindObjectOfType<LevelController>().CurrentTime;
        Debug.Log(timeToShow);
        TimeSpan time = TimeSpan.FromSeconds(timeToShow);
       
        if (Win)
        {
            WinLoseText.text = WinText;
            WinImage.SetActive(true);
            LoseImage.SetActive(false);
            EndScreenTimerText.text = "You disarmed the bomb in: " + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
        }
        else
        {
            //LoseText.SetActive(true);
            WinImage.SetActive(false);
            LoseImage.SetActive(true);
            EndScreenTimerText.text = "You lost in: " + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
        }
    }

    public void Mainmenu()
    {
        FindObjectOfType<LevelController>().ResetEnemyLists();
        FindObjectOfType<LevelController>().HealthCarriedBetweenLevels = FindObjectOfType<LevelController>().MaxHealth;
        FindObjectOfType<LevelController>().CurrentTime = FindObjectOfType<LevelController>().TimerAmount;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
