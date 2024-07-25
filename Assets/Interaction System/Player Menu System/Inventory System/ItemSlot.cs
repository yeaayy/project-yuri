using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemSO item { get; private set; }

    public int itemQuantity;

    public bool isFull
    {
        get { return itemQuantity >= maxNumberOfItems; }
    }

    public bool isEmpty
    {
        get { return itemQuantity == 0; }
    }

    public Sprite emptySprite;

    [SerializeField] private int maxNumberOfItems;

    [SerializeField] private TMP_Text itemQuantityText;
    [SerializeField] private Image itemImage;

    public GameObject selectedShader;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
    }

    public int SetItem(ItemSO item, int itemQuantity)
    {
        this.item = item;
        itemImage.sprite = item.itemSprite;

        int usedQuantity = Math.Min(itemQuantity, maxNumberOfItems);
        SetQuantity(usedQuantity);

        return itemQuantity - usedQuantity;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        inventoryManager.OnClickItemSlot(this);
    }

    public void OnRightClick()
    {
        // Implement any right-click functionality if needed
    }

    public void ClearSlot()
    {
        itemImage.sprite = emptySprite;
        SetQuantity(0);
        itemQuantityText.enabled = false;
    }

    private void SetQuantity(int itemQuantity)
    {
        this.itemQuantity = itemQuantity;
        itemQuantityText.text = itemQuantity.ToString();
        itemQuantityText.enabled = !isEmpty;
    }

}
