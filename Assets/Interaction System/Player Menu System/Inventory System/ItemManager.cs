using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    private List<ItemSO> allItems;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadAllItems()
    {
        allItems = new List<ItemSO>(Resources.LoadAll<ItemSO>("Items"));
    }

    public List<ItemSO> GetAllItems()
    {
        return new List<ItemSO>(allItems);
    }

    public ItemSO GetItemSOByName(string itemName)
    {
        return allItems.Find(item => item.itemName == itemName);
    }

    public List<ItemSO> GetItemsByCategory(ItemSO.ItemCategory category)
    {
        return allItems.FindAll(item => item.category == category);
    }
}