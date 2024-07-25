using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public TMP_Text enemyNameText;
    public List<BattleEnemy> enemies;

    void Start()
    {
        enemyNameText.gameObject.SetActive(false);
    }

    public void OnEnemySelected(BattleEnemy enemy)
    {
        enemyNameText.text = enemy.name;
        enemyNameText.gameObject.SetActive(true);
    }

    public void OnEnemyDeselected()
    {
        enemyNameText.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class BattleEnemy
{
    public string name;

    // Removed the 'new' keyword here
    public int currentHP;
    public int maxHP;
    public GameObject enemyObject;
}