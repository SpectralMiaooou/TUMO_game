using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //Attack variables
    public bool isPrimaryAttackPressed = false;
    public bool isSecondaryAttackPressed = false;
    public bool isUltimateAttackPressed = false;

    //Movement variables
    public bool isRunPressed = false;

    //Jumping variables
    public bool isJumpPressed = false;

    public float scroll;

    public Vector2 mouseInput;

    public Vector2 move;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        
    }
    void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        onMove();
        onJump();
        onPrimaryAttack();
        onSecondaryAttack();
        onUltimateAttack();
        onRun();
        onScroll();
        onMouse();
    }

    void onMove()
	{
        float y_value = (Input.GetKey(KeyCode.UpArrow) == true ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) == true ? 1 : 0);
        float x_value = (Input.GetKey(KeyCode.RightArrow) == true ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) == true ? 1 : 0);
        move = Vector2.up * y_value + Vector2.right * x_value;
    }

    void onJump()
    {
        isJumpPressed = Input.GetKey(KeyCode.Space);
    }

    void onPrimaryAttack()
    {
        isPrimaryAttackPressed = Input.GetKey(KeyCode.E);
    }
    void onSecondaryAttack()
    {
        isSecondaryAttackPressed = Input.GetKey(KeyCode.R);
    }
    void onUltimateAttack()
    {
        isUltimateAttackPressed = Input.GetKey(KeyCode.T);
    }
    void onRun()
    {
        isRunPressed = Input.GetKey(KeyCode.Z);
    }

    void onScroll()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel");
    }

    void onMouse()
    {
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
}