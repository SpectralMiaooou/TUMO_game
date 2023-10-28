using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private bool hasBeenPicked = false;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (!hasBeenPicked && player != null && CanPick(player))
        {
            Pick(player);

            if (DestroyItem())
            {
                Destroy(gameObject);
            }

            hasBeenPicked = true;
        }
    }

    public virtual void Pick(PlayerController controller)
    {
        Debug.Log("Pickable.Pick()");
    }

    public virtual bool CanPick(PlayerController controller)
    {
        return true;
    }

    public virtual bool DestroyItem()
    {
        return true;
    }
}
