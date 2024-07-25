using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemCategory
    {
        Bush,
        Consumable,
        Weapons,
        Armor,
        Misc
    }

    [Header("Item Configuration")]
    public ItemCategory itemCategory; // Dropdown for selecting item category
    public int maxItemQuantity = 10; // Maximum quantity of each item
    public int maxDifferentItems = 5; // Maximum number of different items to be picked up

    private bool isInRange = false; // Flag to track if player is in range
    private bool isPickedUp = false; // Flag to prevent multiple pickups

    private Vector3 initialPosition; // Store initial position of the item

    void Start()
    {
        initialPosition = transform.position; // Store the initial position
    }

    void Update()
    {
        // Check if 'E' key is pressed when player is in range and item has not been picked up
        if (isInRange && !isPickedUp && Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isInRange = true; // Player is in range
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isInRange = false; // Player is no longer in range
        }
    }

    private void PickUpItem()
    {
        // Determine the path based on the selected category
        string path = $"Items/{itemCategory}";

        // Get all ItemSO assets from the specified folder
        ItemSO[] items = Resources.LoadAll<ItemSO>(path);

        // Shuffle the items to ensure randomness
        items = ShuffleItems(items);

        // Pick up items up to the maxDifferentItems limit
        int itemCount = 0;
        foreach (var item in items)
        {
            if (itemCount >= maxDifferentItems)
            {
                break; // Stop if we have reached the limit of different items
            }

            // Randomize the quantity within the specified range
            int quantity = Random.Range(1, maxItemQuantity + 1);

            InventoryManager.Instance.AddItem(item, quantity);
            PromptManager.Instance.ShowPrompt(item.itemSprite, item.itemName, quantity);

            itemCount++;
        }

        Destroy(gameObject); // Remove the item GameObject after picking up
        isPickedUp = true; // Set flag to prevent multiple pickups
    }

    private ItemSO[] ShuffleItems(ItemSO[] items)
    {
        for (int i = items.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (items[i], items[randomIndex]) = (items[randomIndex], items[i]);
        }
        return items;
    }

    private void LateUpdate()
    {
        // Lock position and rotation
        transform.SetPositionAndRotation(initialPosition, Quaternion.identity);
        transform.localScale = Vector3.one; // Uniform scale
    }
}
