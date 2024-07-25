using UnityEngine;
using TMPro;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance { get; private set; }
    public int playerMoney;
    public TMP_Text walletTextMainMenu;
    public TMP_Text walletTextShop;

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
            Destroy(gameObject);
            Debug.Log("Duplicate PlayerMoney Instance destroyed.");
        }
    }

    private void Start()
    {
        UpdateMoneyText();
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
        UpdateMoneyText();
    }

    public void SubtractMoney(int amount)
    {
        playerMoney -= amount;
        UpdateMoneyText();
    }

    public bool CanAfford(int amount)
    {
        return playerMoney >= amount;
    }

    public void UpdateMoneyText()
    {
        if (walletTextMainMenu != null)
        {
            walletTextMainMenu.text = "Money: " + playerMoney.ToString();
            Debug.Log("Money updated in MainMenu: " + playerMoney);
        }

        if (walletTextShop != null)
        {
            walletTextShop.text = "Money: " + playerMoney.ToString();
            Debug.Log("Money updated in Shop: " + playerMoney);
        }
    }
}