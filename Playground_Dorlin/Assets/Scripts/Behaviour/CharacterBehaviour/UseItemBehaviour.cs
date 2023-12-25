using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemBehaviour : MonoBehaviour
{
    Animator anim;

    //Profile Variables
    public UserProfile profile = new UserProfile();
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        profile.username = transform.tag;
        profile.user = gameObject;
        profile.anim = anim;
    }

    void Update()
    {
        //behaviour.LaunchAttack();
    }

    public void UseItem(GameObject weapon, InputHandler input)
    {
        if (input.isPrimaryAttackPressed)
        {
            if (weapon.TryGetComponent<ISwingable>(out ISwingable swingable))
            {
                swingable?.Swing(profile);
            }
            else if (weapon.TryGetComponent<IHitscan>(out IHitscan hitscan))
            {
                hitscan?.Shoot(profile);
            }
            else if (weapon.TryGetComponent<IProjectile>(out IProjectile projectile))
            {
                projectile?.Throw(profile);
            }
        }
        else if (input.isSecondaryAttackPressed)
        {
            //attack.handleAttack(2);
        }
        else if (input.isUltimateAttackPressed)
        {
            //attack.handleAttack(3);
        }
    }
}
