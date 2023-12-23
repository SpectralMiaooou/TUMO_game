using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBehaviour : MonoBehaviour
{
    [SerializeField]
    private WeaponController[] slots = new WeaponController[3];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(WeaponController weapon)
    {
        for (int i = 1; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = weapon;
            }
        }
    }
}
