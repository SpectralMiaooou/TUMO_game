using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class Database : MonoBehaviour
{
    public ItemDatabase items;
    public WeaponDatabase weapons;

    //HealthBar variables
    public Image healthBar;

    public PlayerController player;

    public Transform trash;

    public static Database instance;

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
    void Update()
    {
        handleHealthBar(player.health.healthLife, player.health.maxHealthLife);
    }

    public static Item GetItemByID(string ID)
    {
        return instance.items.allItems.FirstOrDefault(instance => instance.itemID == ID);
    }

    public static Item GetRandomItem(string ID)
    {
        return instance.items.allItems[Random.Range(0, instance.items.allItems.Count())];
    }

    void handleHealthBar(float health, float healthMax)
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (health / healthMax), 3f * Time.deltaTime);

        Color healthColor = Color.Lerp(Color.red, Color.green, (health / healthMax));
        healthBar.color = healthColor;
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
