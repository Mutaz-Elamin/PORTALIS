using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController Controller;
    private HungerSystem hungerSystem; 

    private Vector3 playerVelocity;
    public float speed = 5f;
    private bool isGrounded;
    public float Gravity = -9.8f;
    public float JumpHeight = 1.5f;
    private CameraMovement camMove;
    
    private bool isMoving = false; 

    void Start()
    {
        Controller = GetComponent<CharacterController>();
        camMove = GetComponent<CameraMovement>();
        hungerSystem = GetComponent<HungerSystem>();
        if (hungerSystem == null)
        {
            Debug.LogError("HungerSystem script not found on the Player!");
        }
    }

    void Update()
    {
        isGrounded = Controller.isGrounded;
        
        if (isMoving && isGrounded)
        {
            float depletionRate = (speed > 5f) 
                                ? hungerSystem.sprintDepletionRate 
                                : hungerSystem.walkDepletionRate;
            
            hungerSystem.DepleteHunger(depletionRate * Time.deltaTime);
        }
    }

    public void Move(Vector2 input)
    {
        Vector3 moveDir = Vector3.zero;

        moveDir.x = input.x;
        moveDir.z = input.y;
        
        isMoving = moveDir.magnitude > 0.1f;

        Controller.Move(transform.TransformDirection(moveDir) * speed * Time.deltaTime);
        playerVelocity.y += Gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0) {
            playerVelocity.y = -2f;
        }
        Controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(JumpHeight * -1.5f * Gravity);
        }
    }

    public void StartSprinting()
    {
        speed = 8f;
        camMove.isSprinting = true;
    }

    public void StopSprinting()
    {
        speed = 5f;
        camMove.isSprinting = false;
    }
}