using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PartyMemberUI[] partyMemberUIs;
    public EnemyUI[] enemyUIs;

    private static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static PartyMemberUI GetPartyMemberUI(int index)
    {
        if (instance == null || index >= instance.partyMemberUIs.Length)
            return null;
        return instance.partyMemberUIs[index];
    }

    public static EnemyUI GetEnemyUI(int index)
    {
        if (instance == null || index >= instance.enemyUIs.Length)
            return null;
        return instance.enemyUIs[index];
    }
}