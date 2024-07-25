using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;

    private ItemSO currentItem;
    private System.Action<ItemSO> onItemClicked;
    private ItemDetailsUI itemDetailsUI;

    public void Initialize(ItemSO item, System.Action<ItemSO> onClick, ItemDetailsUI detailsUI)
    {
        currentItem = item;
        itemImage.sprite = item.itemSprite;
        itemNameText.text = item.itemName;
        itemPriceText.text = item.itemPrice.ToString();
        onItemClicked = onClick;
        itemDetailsUI = detailsUI;

        // Ensure that the EventTrigger component is set up
        SetupEventTrigger();
    }

    private void SetupEventTrigger()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        trigger.triggers.Clear();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((eventData) => { onItemClicked(currentItem); });
        trigger.triggers.Add(entry);
    }
}