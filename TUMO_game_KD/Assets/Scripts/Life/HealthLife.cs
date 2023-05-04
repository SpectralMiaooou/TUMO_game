using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthLife : MonoBehaviour
{
    //HealthLife variables
    private float maxHealthLife = 100f;
    private float healthLife;
    public TextMeshProUGUI UIHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthLife = maxHealthLife;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void handleHealthBar()
    {

    }

    public void TakeDamage(float _damage)
    {
        healthLife -= _damage;
        print("Dégats");
    }
}
