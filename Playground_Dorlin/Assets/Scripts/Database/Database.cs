using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Database : MonoBehaviour
{
    public ItemDatabase items;
    public WeaponDatabase weapons;

    private static Database instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Item GetItemByID(string ID)
    {
        return instance.items.allItems.FirstOrDefault(instance => instance.itemID == ID);
    }

    public static Item GetRandomItem(string ID)
    {
        return instance.items.allItems[Random.Range(0, instance.items.allItems.Count())];
    }
    /*
    public static WeaponItem GetWeaponByID(string ID)
    {
        return instance.weapons.allWeapons.FirstOrDefault(instance => instance.weaponID == ID);
    }

    public static WeaponItem GetRandomWeapon(string ID)
    {
        return instance.weapons.allWeapons[Random.Range(0, instance.weapons.allWeapons.Count())];
    }*/
}
