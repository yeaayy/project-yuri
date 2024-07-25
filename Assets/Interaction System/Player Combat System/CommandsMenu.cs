using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandsMenu : MonoBehaviour
{
    public Button attackButton;
    public Button skillButton;
    public Button guardButton;
    public Button itemButton;

    public void ShowMenu()
    {
        gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        gameObject.SetActive(false);
    }
}