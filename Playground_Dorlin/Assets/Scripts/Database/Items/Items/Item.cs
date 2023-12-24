using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Base Options")]
    public string itemID;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public int itemPrice;
    public Sprite itemSprite;
}
