using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBase
{
    public InventorySlot[] inventory;
    public InventorySlot currentItem;
    private int currentIndexItem;
    private int inventorySize;

    public InventoryBase(int size)
    {
        this.inventorySize = size;
        this.inventory = new InventorySlot[size];

        for (int slot = 0; slot < size; slot++)
        {
            this.createDefaultSlot(slot);
        }

        this.currentIndexItem = 0;
        this.currentItem = this.inventory[0];
    }

    public void ChangeSlot(float scroll)
    {
        int lastIndexWeapon = this.currentIndexItem;
        this.currentIndexItem = this.currentIndexItem + (int)(scroll*10);

        if (this.currentIndexItem < 0)
        {
            this.currentIndexItem = this.inventorySize - 1;
        }
        else if (this.currentIndexItem >= this.inventorySize)
        {
            this.currentIndexItem = 0;
        }
        //print(currentIndexItem);
        this.currentItem = inventory[this.currentIndexItem];

        SwitchItem(lastIndexWeapon, this.currentIndexItem);
    }

    public void SearchEmptySlot(GameObject item, Item data, int quantity)
    {
        for (int slot = 0; slot < inventory.Length; slot++)
        {
            if (inventory[slot].quantity == 0)
            {
                AddItem(slot, item, data, quantity);
                return;
            }
        }
    }

    private void SwitchItem(int currentSlot, int nextSlot)
    {
        DisableItem(currentSlot);
        EnableItem(nextSlot);
    }

    private void EnableItem(int slot)
    {
        inventory[slot].item.SetActive(true);
    }
    private void DisableItem(int slot)
    {
        inventory[slot].item.SetActive(false);
    }
    public void AddItem(int slot, GameObject item, Item data, int quantity)
    {
        InventorySlot itemSlot = this.inventory[slot];

        if(itemSlot.quantity == 0)
        {
            itemSlot.item = item;
            itemSlot.data = data;
            itemSlot.quantity += quantity;
            if(currentIndexItem == slot)
            {
                EnableItem(slot);
            }
            else
            {
                DisableItem(slot);
            }
        }
        else
        {
            if (itemSlot.data == data)
            {
                itemSlot.quantity += quantity;
            }
        }
    }
    public void RemoveItem(int slot, int quantity)
    {
        InventorySlot itemSlot = this.inventory[slot];

        itemSlot.quantity -= quantity;
        itemSlot.quantity = Mathf.Clamp(itemSlot.quantity, 0, itemSlot.data.itemMaxCapacity);
        if(itemSlot.quantity == 0)
        {
            createDefaultSlot(slot);
        }
    }

    private void createDefaultSlot(int slot)
    {
        this.inventory[slot] = null;
    }
}
