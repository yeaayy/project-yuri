using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public PartyMemberUI partyMemberUI;

    public override void Initialize(int hp, int maxHp, int sp, int maxSp, bool player, Sprite portraitSprite, MonoBehaviour uiComponent)
    {
        base.Initialize(hp, maxHp, sp, maxSp, player, portraitSprite, uiComponent);
        partyMemberUI = (PartyMemberUI)uiComponent;
        partyMemberUI.SetCharacter(this);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        partyMemberUI.UpdateHPText(currentHP, maxHP);
    }
}