using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDamageManager : MonoBehaviour
{
    public string user;
    public float damage;
    public float height;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        DrawCapsule(transform.position, radius, Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DrawCapsule(Vector3 _pos, float _radius, Color _color = default(Color))
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(_pos, _radius);
    }

    void handleAttackDamage(float _damage)
    {
        damage = _damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HealthLife>() != null)
        {
            //
        }
    }
}
