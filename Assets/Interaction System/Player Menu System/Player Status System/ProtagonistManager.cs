using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistManager : MonoBehaviour
{
    public static ProtagonistManager Instance { get; private set; }
    public Sprite protagonistSprite;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sprite GetProtagonistSprite()
    {
        return protagonistSprite;
    }
}