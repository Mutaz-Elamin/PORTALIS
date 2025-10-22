using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFootActions;

    private PlayerController controller;
    private CameraMovement camMove;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFootActions = playerInput.OnFoot;
        controller = GetComponent<PlayerController>();
        camMove = GetComponent<CameraMovement>();
        onFootActions.Jump.performed += ctx => controller.Jump();

        onFootActions.Sprint.performed += ctx => controller.StartSprinting();
        onFootActions.Sprint.canceled += ctx => controller.StopSprinting();

        onFootActions.LightAttack.performed += ctx => controller.Attack();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        controller.Move(onFootActions.Movement.ReadValue<Vector2>());
        
    }

    private void LateUpdate()
    {
        camMove.Look(onFootActions.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFootActions.Enable();
    }

    private void OnDisable()
    {
        onFootActions.Disable();
    }
}
