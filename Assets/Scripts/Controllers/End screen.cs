using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public Text endMessageText;

    public void Mainmenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
         public void QuitGame()
    {
        Application.Quit();
    }
}
