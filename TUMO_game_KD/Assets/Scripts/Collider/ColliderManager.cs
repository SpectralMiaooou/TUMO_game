using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public GameObject leftHandCollider;
    public GameObject rightHandCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnableLeftHand()
    {
        leftHandCollider.SetActive(true);
    }

    void EnableRightHand()
    {
        rightHandCollider.SetActive(true);
    }

    void DisableLeftHand()
    {
        leftHandCollider.SetActive(false);
    }

    void DisableRightHand()
    {
        rightHandCollider.SetActive(false);
    }
}
