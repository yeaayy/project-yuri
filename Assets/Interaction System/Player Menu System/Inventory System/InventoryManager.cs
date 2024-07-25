using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public GameObject inventoryPanel;
    public GameObject inventoryTabMenu; // Reference to the Inventory Tab Menu GameObject
    public ItemSlot[] itemSlots;
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;

    private List<InventoryData> inventories = new List<InventoryData>();
    private ItemSlot activeSlot = null;
    private ItemSO.ItemCategory categoryFilter = ItemSO.ItemCategory.All;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        HideInventory(); // Ensure it's hidden initially
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        inventoryTabMenu.SetActive(true); // Show the) inventory tab menu when the inventory is shown
        Time.timeScale = 0;
        // Open all item tab by default when inventory opened
        SetCategoryFilter(ItemSO.ItemCategory.All);
    }

    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
        inventoryTabMenu.SetActive(false); // Hide the inventory tab menu when the inventory is hidden
        Time.timeScale = 1;
    }

    public void UseItem(ItemSO item)
    {
        AddItem(item, -1);
    }

    public void AddItem(ItemSO item, int itemQuantity)
    {
        bool found = false;
        for (int i = 0; i < inventories.Count; i++)
        {
            InventoryData inventory = inventories[i];
            if (inventory.item != item)
                continue;

            found = true;
            inventory.quantity += itemQuantity;
            if (inventory.quantity == 0)
                inventories.RemoveAt(i);
            break;
        }
        if (!found)
            inventories.Add(new InventoryData(item, itemQuantity));
        UpdateUI();
    }

    public void OnClickItemSlot(ItemSlot slot)
    {
        // Deactive previous active slot if theres any.
        this.activeSlot?.selectedShader.SetActive(false);

        // Use the item if it clicked again.
        if (slot != null && this.activeSlot == slot)
        {
            UseItem(slot.item);
            slot = null;
        }
        this.activeSlot = slot;
        if (slot == null)
        {
            itemDescriptionNameText.text = null;
            itemDescriptionText.text = null;
            itemDescriptionImage.sprite = null;
        }
        else
        {
            ItemSO item = slot.item;
            itemDescriptionNameText.text = item.itemName;
            itemDescriptionText.text = item.itemDescription;
            itemDescriptionImage.sprite = item.itemSprite;
            slot.selectedShader.SetActive(true);
        }
    }

    public void SetCategoryFilter(ItemSO.ItemCategory category)
    {
        this.categoryFilter = category;
        this.OnClickItemSlot(null);
        this.UpdateUI();
    }

    private void UpdateUI()
    {
        if (inventories.Count > itemSlots.Length)
            Debug.LogError("Item in inventory exceed number of available slots.");

        int slotIndex = 0;

        // Fill all available slots with the inventory.
        foreach (var inventory in inventories)
        {
            // Dont show the inventory item if it doesn't match the filter.
            if (categoryFilter != ItemSO.ItemCategory.All && inventory.item.category != categoryFilter)
                continue;

            int itemQuantity = inventory.quantity;
            while ((itemQuantity = itemSlots[slotIndex++].SetItem(inventory.item, itemQuantity)) > 0
                    && slotIndex < itemSlots.Length) ;
        }

        // Clear the remaining slots.
        while (slotIndex < itemSlots.Length)
            itemSlots[slotIndex++].ClearSlot();

    }

}
