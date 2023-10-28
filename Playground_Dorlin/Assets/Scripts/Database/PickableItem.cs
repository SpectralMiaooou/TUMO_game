using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : Pickable
{
    [SerializeField]
    private Item item = null;

    [SerializeField]
    private int amount = 1;

    public override void Pick( PlayerController controller)
    {
        //controller.inventory.Add(item, amount);
    }
}
