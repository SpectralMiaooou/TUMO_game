using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBehaviour : MonoBehaviour, IInteractable
{
    public GameObject item;
    public int quantity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Interact(Transform playerGO)
    {
        PlayerController player = playerGO.GetComponent<PlayerController>();
        Item data = item.GetComponent<IItem>().GetItem();
        player.inventory.SearchEmptySlot(item, data, quantity);
        item.transform.SetParent(player.target);
        item.transform.localPosition = Vector3.zero;
        item.transform.rotation = Quaternion.identity;
        //item.transform.localScale = Vector3.one;
        Destroy(gameObject);
    }
}
