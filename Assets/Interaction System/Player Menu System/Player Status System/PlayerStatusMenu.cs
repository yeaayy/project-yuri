using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusMenu : MonoBehaviour
{
    public GameObject playerStatusPanel; // Assign the playerStatusPanel in the Inspector
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Slider hpBar;
    public TextMeshProUGUI hpText;
    public Slider spBar;
    public TextMeshProUGUI spText;
    public TextMeshProUGUI statsText;

    private PlayerStats playerStats; // Assuming you have a PlayerStats script managing player data

    private void Start()
    {
        // Find and assign the PlayerStats script
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        // Ensure the status menu is initially hidden
        playerStatusPanel.SetActive(false);
    }

    public void OpenPlayerStatusMenu()
    {
        UpdatePlayerStatus();
        playerStatusPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ClosePlayerStatusMenu()
    {
        playerStatusPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void UpdatePlayerStatus()
    {
        nameText.text = playerStats.characterName;
        levelText.text = "Lv " + playerStats.level.ToString();
        hpText.text = $"{playerStats.currentHP} / {playerStats.maxHP}";
        hpBar.value = (float)playerStats.currentHP / playerStats.maxHP;
        spText.text = $"{playerStats.currentSP} / {playerStats.maxSP}";
        spBar.value = (float)playerStats.currentSP / playerStats.maxSP;

        // Update stats
        statsText.text = $"Attack: {playerStats.attack}\nDefense: {playerStats.defense}";
    }

    // Example method to simulate the player taking damage
    public void TakeDamage(int damageAmount)
    {
        playerStats.currentHP -= damageAmount;
        if (playerStats.currentHP < 0)
            playerStats.currentHP = 0;

        UpdatePlayerStatus();

        // Check if player has died
        if (playerStats.currentHP <= 0)
        {
            // Handle player death (e.g., respawn, game over, etc.)
            Debug.Log("Player has died!");
        }
    }

    // Example method to simulate regenerating health and energy
    public void RegenerateHealthAndEnergy(int hpAmount, int spAmount)
    {
        playerStats.currentHP += hpAmount;
        if (playerStats.currentHP > playerStats.maxHP)
            playerStats.currentHP = playerStats.maxHP;

        playerStats.currentSP += spAmount;
        if (playerStats.currentSP > playerStats.maxSP)
            playerStats.currentSP = playerStats.maxSP;

        UpdatePlayerStatus();
    }
}