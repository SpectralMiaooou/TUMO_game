using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public GameObject item;
    public Item data;
    public int quantity; 
    /*
    public InventorySlot()
    {
        item = Resources.Load<GameObject>("Assets/Prefabs/Weapons/Default.prefab");
        data = Resources.Load<MeleeWeaponItem>("Assets/Database/Weapons/Default.asset");
    }*/
}
