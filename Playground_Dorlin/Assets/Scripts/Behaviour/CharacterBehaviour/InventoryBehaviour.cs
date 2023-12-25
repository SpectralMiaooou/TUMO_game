using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBehaviour : MonoBehaviour
{
    public InventorySlot[] inventory = new InventorySlot[3];
    public InventorySlot currentWeapon;
    public int currentIndexWeapon;
    // Start is called before the first frame update
    void Start()
    {
        for (int slot = 0; slot < inventory.Length; slot++)
        {
            createDefaultSlot(slot);
        }
        currentIndexWeapon = 0;
        currentWeapon = inventory[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSlot(float scroll)
    {
        int lastIndexWeapon = currentIndexWeapon;
        currentIndexWeapon = currentIndexWeapon + (int)(scroll*10);
        if (currentIndexWeapon < 0)
        {
            currentIndexWeapon = inventory.Length - 1;
        }
        else if (currentIndexWeapon >= inventory.Length)
        {
            currentIndexWeapon = 0;
        }
        print(currentIndexWeapon);
        currentWeapon = inventory[currentIndexWeapon];

        SwitchItem(lastIndexWeapon, currentIndexWeapon);
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
        if(inventory[slot].quantity == 0)
        {
            Destroy(inventory[slot].item);
            inventory[slot].item = item;
            inventory[slot].data = data;
            inventory[slot].quantity += quantity;
        }
        else
        {
            if (inventory[slot].data == data)
            {
                inventory[slot].quantity += quantity;
            }
        }
    }
    public void RemoveItem(int slot, int quantity)
    {
        inventory[slot].quantity = Mathf.Clamp(quantity, 0, inventory[slot].data.itemMaxCapacity);
        if(inventory[slot].quantity == 0)
        {
            createDefaultSlot(slot);
        }
    }

    private void createDefaultSlot(int slot)
    {
        GameObject go = Instantiate(Database.instance.weapons.allWeapons[0].weaponPrefab, transform.position, Quaternion.identity);
        go.transform.SetParent(Database.instance.trash);
        AddItem(slot, go, Database.instance.weapons.allWeapons[0], 0);
    }
}
