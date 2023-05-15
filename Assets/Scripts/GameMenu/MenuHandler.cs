using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public void StartGame()
    {
        // load the game
        if (PlayerPrefs.HasKey("TimeValue"))
        {
            if (PlayerPrefs.GetInt("TimeValue") == 1)
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void RestartGame()
    {
        // restart the game by reloading the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        // return to the main menu
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        // quit the game
        Application.Quit();
    }
}
