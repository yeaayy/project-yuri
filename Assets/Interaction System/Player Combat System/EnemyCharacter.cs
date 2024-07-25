using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    public EnemyUI enemyUI;

    public override void Initialize(int hp, int maxHp, int sp, int maxSp, bool player, Sprite portraitSprite, MonoBehaviour uiComponent)
    {
        base.Initialize(hp, maxHp, sp, maxSp, player, portraitSprite, uiComponent);
        enemyUI = (EnemyUI)uiComponent;
        enemyUI.SetCharacter(this);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        enemyUI.UpdateHPText(currentHP, maxHP);
        enemyUI.Show();
        StartCoroutine(HideAfterDelay(2)); // Hide after 2 seconds
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyUI.Hide();
    }
}