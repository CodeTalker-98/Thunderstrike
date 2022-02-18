﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private GameInputActions _inputs;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _playerMovement = GetComponentInChildren<PlayerMovement>();

        _inputs = new GameInputActions();
        _inputs.Player.Enable();
        InitializePerformedActions();
    }

    private void InitializePerformedActions()
    {
        _inputs.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //Pause Game from game manager??
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (_playerMovement != null)
        {
            Vector2 input = _inputs.Player.Movement.ReadValue<Vector2>();
            _playerMovement.CalculateMovement(input);
        }
    }
}
