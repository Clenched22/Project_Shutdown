using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public GameObject mainMenuPanel;
    public GameObject tutorialPanel;
    public GameObject creditsPanel;

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    public void ShowTutorial()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    
    public void OnMainMenu()
    {
        ShowMainMenu();
    }


}

