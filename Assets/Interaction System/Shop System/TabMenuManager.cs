using UnityEngine;
using UnityEngine.UI;

public class TabMenuManager : MonoBehaviour
{
    public ShopManager shopManager;

    private void Start()
    {
        transform.Find("AllButton").GetComponent<Button>().onClick.AddListener(() => shopManager.PopulateShop(ItemSO.ItemCategory.All));
        transform.Find("ItemButton").GetComponent<Button>().onClick.AddListener(() => shopManager.PopulateShop(ItemSO.ItemCategory.Item));
        transform.Find("WeaponButton").GetComponent<Button>().onClick.AddListener(() => shopManager.PopulateShop(ItemSO.ItemCategory.Weapon));
        transform.Find("EquipmentButton").GetComponent<Button>().onClick.AddListener(() => shopManager.PopulateShop(ItemSO.ItemCategory.Equipment));
        transform.Find("AccButton").GetComponent<Button>().onClick.AddListener(() => shopManager.PopulateShop(ItemSO.ItemCategory.Acc));
    }
}