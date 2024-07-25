using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoneyInitializer : MonoBehaviour
{
    private void Start()
    {
        PlayerMoney playerMoney = FindObjectOfType<PlayerMoney>();
        if (playerMoney != null)
        {
            playerMoney.gameObject.SetActive(true);
            playerMoney.UpdateMoneyText();
            Debug.Log("PlayerMoney found and initialized.");
        }
        else
        {
            Debug.LogError("PlayerMoney not found in the scene.");
        }
    }
}