using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public void StartGame()
    {
        // Load the main game scene
        SceneManager.LoadScene("MainScene");
    }

    public void OpenOptions()
    {
        // Open the options menu
        // Implement this function based on your options menu design
    }

    public void ExitGame()
    {
        // Exit the game
        Application.Quit();
    }
}