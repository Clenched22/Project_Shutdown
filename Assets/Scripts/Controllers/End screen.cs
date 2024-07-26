using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{


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
