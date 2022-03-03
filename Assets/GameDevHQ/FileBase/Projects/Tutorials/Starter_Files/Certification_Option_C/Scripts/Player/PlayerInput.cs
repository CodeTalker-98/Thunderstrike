using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private GameInputActions _inputs;

    private bool _isPaused = false;

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
        if (GameManager.instance != null)
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                GameManager.instance.Pause();
            }
            else
            {
                GameManager.instance.Resume();
            }
        }
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
