using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnShot;

    private PlayerInputAction playerInputAction;

    private void Awake()
    {
        Instance = this;
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Shoot.performed += Shoot_started;
        
    }

    private void Shoot_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnShot?.Invoke(this, EventArgs.Empty);
    }

    void OnEnable()
    {
        playerInputAction.Enable();
    }

    void OnDisable()
    {
        playerInputAction.Disable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public Vector2 GetRotationVectorNormalized()
    {
        Vector2 inputVector = playerInputAction.Player.Rotate.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
