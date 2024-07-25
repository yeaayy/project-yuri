using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackMessage : MonoBehaviour
{
    public TMP_Text messageText;

    private void Start()
    {
        HideMessage(); // Start hidden
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
    }

    public void HideMessage()
    {
        messageText.gameObject.SetActive(false);
    }
}