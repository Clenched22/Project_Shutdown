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
    [SerializeField] string LoseText;
    [SerializeField] GameObject WinImage;
    [SerializeField] GameObject LoseImage;

    private void Start()
    {
        Win = FindObjectOfType<LevelController>().Win;
        if (Win)
        {
            WinLoseText.text = WinText;
            WinImage.SetActive(true);
            LoseImage.SetActive(false);
        }
        else
        {
            WinLoseText.text = LoseText;
            WinImage.SetActive(false);
            LoseImage.SetActive(true);
        }
        float timeToShow = FindObjectOfType<LevelController>().TimerAmount;
        timeToShow -= FindObjectOfType<LevelController>().CurrentTime;
        Debug.Log(timeToShow);
        TimeSpan time = TimeSpan.FromSeconds(timeToShow);
        EndScreenTimerText.text = "It Took " + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
        EndScreenTimerText.enabled = true;
    }

    void Update()
    {
        float timeToShow = FindObjectOfType<LevelController>().TimerAmount;
        timeToShow -= FindObjectOfType<LevelController>().CurrentTime;
        Debug.Log(timeToShow);
        TimeSpan time = TimeSpan.FromSeconds(timeToShow);
        EndScreenTimerText.text = "You disarmed the bomb in " + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
        if (Win)
        {
            WinLoseText.text = WinText;
            WinImage.SetActive(true);
            LoseImage.SetActive(false);
        }
        else
        {
            WinLoseText.text = LoseText;
            WinImage.SetActive(false);
            LoseImage.SetActive(true);
        }
    }

    public void Mainmenu()
    {
        FindObjectOfType<LevelController>().HealthCarriedBetweenLevels = FindObjectOfType<LevelController>().MaxHealth;
        FindObjectOfType<LevelController>().CurrentTime = FindObjectOfType<LevelController>().TimerAmount;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
