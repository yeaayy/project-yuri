using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(1); // Change the number based on what it is in the build settings
    }
}