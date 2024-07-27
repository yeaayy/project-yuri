using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MenuController : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject playerStatusPanel;
    public GameObject calendarPanel;
    public TMP_Text walletTextMainMenu;
    public TMP_Text[] menuOptions;
    public GameObject protagonistPortrait;
    public GameObject currentWeaponIcon;
    private int selectedIndex = 0;
    private bool isMenuActive = false;

    private GameObject activeSubMenu = null;

    void Start()
    {
        menuUI.SetActive(true); // Temporarily activate the menu to initialize the TMP object
        playerStatusPanel.SetActive(false);
        calendarPanel.SetActive(false);

        this.UpdateMoneyText(PlayerMoney.Instance);
        EventBus.Register<PlayerMoney>(PlayerMoney.ChangedEvent, this.UpdateMoneyText);

        menuUI.SetActive(false); // Deactivate the menu if needed
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMenu();
        }

        if (isMenuActive)
        {
            HandleMenuNavigation();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscape();
        }
    }

    void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        menuUI.SetActive(isMenuActive);

        if (isMenuActive)
        {
            UpdateProtagonistPortrait();
            UpdateCurrentWeaponIcon();
        }

        if (!isMenuActive)
        {
            if (activeSubMenu != null)
            {
                activeSubMenu.SetActive(false);
                activeSubMenu = null;
            }
            InventoryManager.Instance.HideInventory(); // Hide the inventory when the menu is toggled off
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }

        selectedIndex = 0;
        UpdateMenu();
    }

    void HandleMenuNavigation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex--;
            if (selectedIndex < 0) selectedIndex = menuOptions.Length - 1;
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex++;
            if (selectedIndex >= menuOptions.Length) selectedIndex = 0;
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectOption();
        }
    }

    void UpdateMenu()
    {
        for (int i = 0; i < menuOptions.Length; i++)
        {
            menuOptions[i].color = i == selectedIndex ? Color.red : Color.white;
        }
    }

    void SelectOption()
    {
        switch (selectedIndex)
        {
            case 0:
                // Assuming 0 is the index for some action, update accordingly
                break;
            case 1:
                ShowInventory();
                break;
            case 2:
                OpenPlayerStatus();
                break;
            case 3:
                ShowCalendar();
                break;
            default:
                break;
        }
    }

    void ShowInventory()
    {
        ToggleMenu();
        InventoryManager.Instance.ShowInventory();
        activeSubMenu = InventoryManager.Instance.inventoryPanel;
    }

    void OpenPlayerStatus()
    {
        ToggleMenu();
        playerStatusPanel.SetActive(true);
        activeSubMenu = playerStatusPanel;
    }

    void ShowCalendar()
    {
        ToggleMenu();
        calendarPanel.SetActive(true);
        activeSubMenu = calendarPanel;
    }

    void HandleEscape()
    {
        if (activeSubMenu != null)
        {
            activeSubMenu.SetActive(false);
            activeSubMenu = null;
            Time.timeScale = 1;
            menuUI.SetActive(true);
            isMenuActive = true;
        }
        else if (isMenuActive)
        {
            ToggleMenu();
        }
    }

    void UpdateProtagonistPortrait()
    {
        Sprite protagonistSprite = ProtagonistManager.Instance.GetProtagonistSprite();
        protagonistPortrait.GetComponent<UnityEngine.UI.Image>().sprite = protagonistSprite;
    }

    void UpdateCurrentWeaponIcon()
    {
        Sprite weaponSprite = WeaponManager.Instance.GetCurrentWeaponSprite();
        currentWeaponIcon.GetComponent<UnityEngine.UI.Image>().sprite = weaponSprite;
    }

    private void UpdateMoneyText(PlayerMoney playerMoney)
    {
        walletTextMainMenu.text = "Money: " + playerMoney.playerMoney.ToString();
    }
}
