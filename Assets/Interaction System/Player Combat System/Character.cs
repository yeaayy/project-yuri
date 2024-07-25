using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int currentHP;
    public int maxHP;
    public int currentSP;
    public int maxSP;
    public bool isPlayer;
    public Sprite portrait;

    public virtual void Initialize(int hp, int maxHp, int sp, int maxSp, bool player, Sprite portraitSprite, MonoBehaviour uiComponent)
    {
        currentHP = hp;
        maxHP = maxHp;
        currentSP = sp;
        maxSP = maxSp;
        isPlayer = player;
        portrait = portraitSprite;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
    }
}