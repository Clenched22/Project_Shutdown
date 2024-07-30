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
    [SerializeField] TMP_Text TimerText;
    [SerializeField] string WinText;
    [SerializeField] string LoseText;

    private void Start()
    {
        Win = FindObjectOfType<LevelController>().Win;
        if (Win)
        {
            WinLoseText.text = WinText;
        }
        else
        {
            WinLoseText.text = LoseText;
        }
        float timeToShow = FindObjectOfType<LevelController>().TimerAmount;
        timeToShow -= FindObjectOfType<LevelController>().CurrentTime;
        Debug.Log(timeToShow);
        TimeSpan time = TimeSpan.FromSeconds(timeToShow);
        TimerText.text = "You disarmed the bomb in " + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
        TimerText.enabled = true;
    }

    void Update()
    {
        float timeToShow = FindObjectOfType<LevelController>().TimerAmount;
        timeToShow -= FindObjectOfType<LevelController>().CurrentTime;
        Debug.Log(timeToShow);
        TimeSpan time = TimeSpan.FromSeconds(timeToShow);
        TimerText.text = "You disarmed the bomb in " + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
        if (Win)
        {
            WinLoseText.text = WinText;
        }
        else
        {
            WinLoseText.text = LoseText;
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
