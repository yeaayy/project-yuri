using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance { get; private set; }
    public static string ChangedEvent = "PlayerMoneyChanged";
    public int playerMoney;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the GameObject persists between scenes
            Debug.Log("PlayerMoney Instance created.");
        }
        else
        {
            Debug.Log("Duplicate PlayerMoney Instance destroyed.");
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Debug.Log("PlayerMoney GameObject is enabled.");
    }

    private void OnDisable()
    {
        Debug.Log("PlayerMoney GameObject is disabled.");
    }

    private void OnDestroy()
    {
        Debug.Log("PlayerMoney GameObject is destroyed.");
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        EventBus.Trigger(ChangedEvent, this);
    }

    public void SubtractMoney(int amount)
    {
        AddMoney(-amount);
    }

    public bool CanAfford(int amount)
    {
        return playerMoney >= amount;
    }

}
