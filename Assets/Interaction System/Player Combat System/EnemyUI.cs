using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text hpText;

    public void SetCharacter(EnemyCharacter character)
    {
        nameText.text = character.name;
        UpdateHPText(character.currentHP, character.maxHP);
        Hide(); // Start hidden
    }

    public void UpdateHPText(int currentHP, int maxHP)
    {
        hpText.text = $"HP: {currentHP}/{maxHP}";
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void Hide()
    {
        hpText.gameObject.SetActive(false);
    }

    public void Show()
    {
        hpText.gameObject.SetActive(true);
    }
}