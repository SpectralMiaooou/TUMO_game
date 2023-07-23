using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Animator variables
    Animator anim;


    //Impact variables
    private Vector3 impactDirection = Vector3.zero;

    //Camera variables
    Transform cam;

    //CharacterController variables
    CharacterController character;

    //Movement variables
    private float actualSpeed;
    public float walkingSpeed = 3f;
    public float runningSpeed = 10f;
    public Vector3 desiredMoveDirection;
    public Vector3 currentMovement;



    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        actualSpeed = walkingSpeed;
    }

    public void handleMovement(Vector2 direction, bool isRunning)
    {
        desiredMoveDirection = Vector3.zero;

        if (isRunning)
        {
            actualSpeed = runningSpeed;
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", true);
        }
        else
        {
            actualSpeed = walkingSpeed;
            anim.SetBool("isWalking", true);
            anim.SetBool("isRunning", false);
        }
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * direction.y + right * direction.x;

        if (direction.magnitude != 0f)
        {
            character.Move(desiredMoveDirection* actualSpeed * Time.deltaTime);
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isMoving", false);
        }
    }

    void handleImpact()
    {
        if (impactDirection.magnitude > 0.2)
        {
            currentMovement += impactDirection;
        }
        // consumes the impact energy each cycle:
        impactDirection = Vector3.Lerp(impactDirection, Vector3.zero, 4 * Time.deltaTime);
    }
}
