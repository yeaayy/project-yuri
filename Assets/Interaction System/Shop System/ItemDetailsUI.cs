using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailsUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemPriceText;
    public Image itemImage;
    public GameObject buyPanel;
    public TextMeshProUGUI quantityText; // Display quantity
    public Button plusButton; // Increment quantity
    public Button minusButton; // Decrement quantity
    public Button buyButton;
    public Button cancelButton;

    private ShopManager shopManager;
    private ItemSO currentItem;
    private int currentQuantity = 1;

    void Start()
    {
        // Ensure ItemDetailsUI is initially inactive
        gameObject.SetActive(false);
    }

    public void SetShopManager(ShopManager manager)
    {
        shopManager = manager;
    }

    public void DisplayItemDetails(ItemSO item)
    {
        currentItem = item;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        itemPriceText.text = item.itemPrice.ToString();
        itemImage.sprite = item.itemSprite;
        currentQuantity = 1; // Reset quantity to 1
        UpdateQuantityText();
        buyPanel.SetActive(true); // Show the buy panel
        gameObject.SetActive(true); // Show the ItemDetailsUI panel
        Debug.Log("Item details displayed for: " + item.itemName);
    }

    public void OnPlusButtonClicked()
    {
        currentQuantity++;
        UpdateQuantityText();
    }

    public void OnMinusButtonClicked()
    {
        if (currentQuantity > 1)
        {
            currentQuantity--;
            UpdateQuantityText();
        }
    }

    public void OnBuyButtonClicked()
    {
        if (currentQuantity > 0)
        {
            shopManager.PurchaseItem(currentItem, currentQuantity);
            buyPanel.SetActive(false);
            gameObject.SetActive(false); // Hide the ItemDetailsUI panel after purchase
            Debug.Log("Item purchase confirmed: " + currentItem.itemName);
        }
        else
        {
            Debug.Log("Invalid quantity entered.");
        }
    }

    public void OnCancelButtonClicked()
    {
        buyPanel.SetActive(false);
        gameObject.SetActive(false); // Hide the ItemDetailsUI panel on cancel
        Debug.Log("Item purchase cancelled.");
    }

    private void UpdateQuantityText()
    {
        quantityText.text = currentQuantity.ToString();
    }
}