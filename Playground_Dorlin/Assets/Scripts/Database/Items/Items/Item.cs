using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Asset/Item")]

public class Item : ScriptableObject
{
    public string itemID;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public int itemCost;
    public Sprite itemSprite;
}
