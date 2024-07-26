using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject MainMenuPanel;
    [SerializeField] GameObject TutorialPanel;
    [SerializeField] GameObject CreditsPanel;
    private bool TutorialActive;
    private bool CreditsActive;

    private void Start()
    {
        TutorialActive = false;
        CreditsActive = false;
        MainMenuPanel.SetActive(true);
        TutorialPanel.SetActive(false);
        CreditsPanel.SetActive(false);
    }

    public void ShowTutorial()
    {
        if (TutorialActive)
        {
            TutorialActive = false;
            MainMenuPanel.SetActive(true);
            TutorialPanel.SetActive(false);
            CreditsPanel.SetActive(false);
        }
        else
        {
            TutorialActive = true;
            MainMenuPanel.SetActive(false);
            TutorialPanel.SetActive(true);
            CreditsPanel.SetActive(false);
        }
    }

    public void ShowCredits()
    {
        if (TutorialActive)
        {
            CreditsActive = false;
            MainMenuPanel.SetActive(true);
            TutorialPanel.SetActive(false);
            CreditsPanel.SetActive(false);
        }
        else
        {
            CreditsActive = true;
            MainMenuPanel.SetActive(false);
            TutorialPanel.SetActive(false);
            CreditsPanel.SetActive(true);
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
        FindObjectOfType<LevelController>().ActualStart();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

