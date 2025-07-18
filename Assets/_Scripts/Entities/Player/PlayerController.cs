using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions input;
    private PlayerMovement movement;
    private PlayerShooting shooting;
    private Vector2 moveDirection;

    private void Awake()
    {
        input = new InputSystem_Actions();
        movement = GetComponent<PlayerMovement>();
        shooting = GetComponent<PlayerShooting>();
    }

    private void OnEnable()
    {
        input.Player.Enable();

        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMoveCanceled;
        input.Player.Dash.performed += OnDash;
    }

    private void OnDisable()
    {
        input.Player.Disable();

        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMoveCanceled;
        input.Player.Dash.performed -= OnDash;
    }

    private void OnMove(InputAction.CallbackContext ctx) => moveDirection = ctx.ReadValue<Vector2>();
    
    private void OnMoveCanceled(InputAction.CallbackContext ctx) => moveDirection = Vector2.zero;
    
    private void OnDash(InputAction.CallbackContext ctx) => movement.Dash(moveDirection);

    private void Update()
    {
        if (input.Player.Attack.IsPressed()) shooting.Shoot();
    }
    private void FixedUpdate()
    {
        movement.Move(moveDirection);
    }

}