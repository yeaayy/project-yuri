using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    public Transform shopSlotsContainer;
    public GameObject shopSlotPrefab;
    public TMP_Text walletTextShop;
    public ItemDetailsUI itemDetailsUI;
    public InventoryManager inventoryManager;

    void Start()
    {
        // Deactivate shop panel initially
        gameObject.SetActive(false);
        UpdateMoneyText(PlayerMoney.Instance);
        EventBus.Register<PlayerMoney>(PlayerMoney.ChangedEvent, this.UpdateMoneyText);

        itemDetailsUI.SetShopManager(this);
    }

    public void PopulateShop(ItemSO.ItemCategory category)
    {
        // Activate shop panel when populating
        gameObject.SetActive(true);

        foreach (Transform child in shopSlotsContainer)
        {
            Destroy(child.gameObject);
        }

        List<ItemSO> items = GetItemsByCategory(category);

        foreach (ItemSO item in items)
        {
            GameObject slot = Instantiate(shopSlotPrefab, shopSlotsContainer);
            ShopSlot shopSlot = slot.GetComponent<ShopSlot>();
            shopSlot.Initialize(item, OnItemSlotClicked, itemDetailsUI);
        }
    }

    private List<ItemSO> GetItemsByCategory(ItemSO.ItemCategory category)
    {
        List<ItemSO> items = new List<ItemSO>();

        ItemSO[] allItems = Resources.LoadAll<ItemSO>("Items");
        foreach (ItemSO item in allItems)
        {
            if (category == ItemSO.ItemCategory.All || item.category == category)
            {
                items.Add(item);
            }
        }
        return items;
    }

    private void OnItemSlotClicked(ItemSO item)
    {
        itemDetailsUI.DisplayItemDetails(item);
    }

    public void PurchaseItem(ItemSO item, int quantity)
    {
        int totalCost = item.itemPrice * quantity;
        if (PlayerMoney.Instance.CanAfford(totalCost))
        {
            PlayerMoney.Instance.SubtractMoney(totalCost);
            inventoryManager.AddItem(item, quantity);
        }
        else
        {
            Debug.Log("Not enough money to purchase item.");
        }
    }

    private void UpdateMoneyText(PlayerMoney playerMoney)
    {
        walletTextShop.text = "Money: " + playerMoney.playerMoney.ToString();
    }
}
