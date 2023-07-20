using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackController : MonoBehaviour
{
    public Attack currentAttack;
    [SerializeField]
    private GameObject prefabHurtbox;
    public CapsuleCollider capsuleCollider;

    public GameObject user;
    public float damage;
    public float maxRange;
    public float radius;
    public float duration;

    public abstract void Start();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HealthLife>() != null && other.gameObject != user)
        {
            HealthLife health = other.GetComponent<HealthLife>();
            health.TakeDamage(damage);
            Debug.Log(other.name + ": " + health.healthLife.ToString());
        }
    }
}
