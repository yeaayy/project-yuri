using UnityEngine;
using UnityEngine.UI;

public class InventoryTabMenuManager : MonoBehaviour
{
    private void Start()
    {
        var inventoryManager = GetComponentInParent<InventoryManager>();

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager reference not set in InventoryTabMenuManager");
            return;
        }

        Debug.Log("InventoryTabMenuManager Start method executed, setting up listeners.");

        Transform allButton = transform.Find("AllButton");
        if (allButton != null)
        {
            allButton.GetComponent<Button>().onClick.AddListener(() => inventoryManager.SetCategoryFilter(ItemSO.ItemCategory.All));
            Debug.Log("AllButton listener assigned.");
        }

        Transform itemButton = transform.Find("ItemButton");
        if (itemButton != null)
        {
            itemButton.GetComponent<Button>().onClick.AddListener(() => inventoryManager.SetCategoryFilter(ItemSO.ItemCategory.Item));
            Debug.Log("ItemButton listener assigned.");
        }

        Transform weaponButton = transform.Find("WeaponButton");
        if (weaponButton != null)
        {
            weaponButton.GetComponent<Button>().onClick.AddListener(() => inventoryManager.SetCategoryFilter(ItemSO.ItemCategory.Weapon));
            Debug.Log("WeaponButton listener assigned.");
        }

        Transform equipmentButton = transform.Find("EquipmentButton");
        if (equipmentButton != null)
        {
            equipmentButton.GetComponent<Button>().onClick.AddListener(() => inventoryManager.SetCategoryFilter(ItemSO.ItemCategory.Equipment));
            Debug.Log("EquipmentButton listener assigned.");
        }

        Transform accButton = transform.Find("AccButton");
        if (accButton != null)
        {
            accButton.GetComponent<Button>().onClick.AddListener(() => inventoryManager.SetCategoryFilter(ItemSO.ItemCategory.Acc));
            Debug.Log("AccButton listener assigned.");
        }

        Transform keyItemButton = transform.Find("KeyItemButton");
        if (keyItemButton != null)
        {
            keyItemButton.GetComponent<Button>().onClick.AddListener(() => inventoryManager.SetCategoryFilter(ItemSO.ItemCategory.KeyItem));
            Debug.Log("KeyItemButton listener assigned.");
        }
    }
}
