using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemBehaviour : MonoBehaviour
{
    public void PickUp(Ray ray)
    {
        RaycastHit hit;
        // Shot ray to find object to pick
        if (Physics.Raycast(ray, out hit, 1.5f))
        {
            // Check if object is pickable
            var pickable = hit.transform.GetComponent<PickableItem>();
            // If object has PickableItem class
            if (pickable)
            {
                // Pick it
                PickItem(pickable);
            }
        }
    }
}
