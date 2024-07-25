using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyMemberUI : MonoBehaviour
{
    public Image portraitImage;
    public TMP_Text hpText;

    private Character character;

    public void SetCharacter(Character character)
    {
        this.character = character;
        UpdateHPText(character.currentHP, character.maxHP);
    }

    public void UpdateHPText(int currentHP, int maxHP)
    {
        if (hpText != null)
        {
            hpText.text = $"{currentHP}/{maxHP} HP";
        }
    }
}