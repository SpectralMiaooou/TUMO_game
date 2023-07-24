using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ChaseBehaviour : MonoBehaviour
{
    CharacterController character;

    //Animation variables
    Animator anim;

    public NavMeshAgent agent;

    //Movement variables
    public float chaseSpeed = 5f;
    private Vector3 desVelocity;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

    }

    public void handleChase(Transform target)
    {
        if(target != null)
        {
            agent.enabled = true;
            anim.SetBool("isMoving", true);
            anim.SetBool("isRunning", true);
            anim.SetBool("isWalking", false);
            agent.speed = chaseSpeed;

            //agent.SetDestination(pos);
            agent.destination = target.position;
            desVelocity = agent.desiredVelocity;

            desVelocity.y = 0f;


            character.Move(desVelocity * Time.deltaTime);

            agent.velocity = character.velocity;
        }
        else
        {
            agent.enabled = false;
            anim.SetBool("isMoving", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
        }
    }
}
