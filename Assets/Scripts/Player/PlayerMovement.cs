using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public bool moving = false;
    public float speed = 5f;

    public Rigidbody2D rb;
    public Camera cam;

    Player playerScript;

    Vector2 movementInput;
    Vector2 mousePos;
    Vector2 smoothedMovementInput;
    Vector2 movementInputSmoothVelocity;

    private void OnMove(InputValue inputValue){
        movementInput = inputValue.Get<Vector2>();
    }

    void Start(){
        playerScript = GetComponent<Player>();
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {   
        if(playerScript.isAlive()){
            smoothedMovementInput = Vector2.SmoothDamp(
            smoothedMovementInput,
            movementInput,
            ref movementInputSmoothVelocity,
            0.04f
        );
        
        rb.velocity = movementInput * speed;

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        }
    }
}
