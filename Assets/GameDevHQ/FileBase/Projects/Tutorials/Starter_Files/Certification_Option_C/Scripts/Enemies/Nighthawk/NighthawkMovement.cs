﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NighthawkMovement : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _movementSpeed = 20.0f;

    private bool _isChangingDirection = false;

    private Vector3 _direction = Vector3.zero;
    private Vector3 _spawnPos = Vector3.zero;

    private Animator _anim;

    private EnemyShoot _enemyShoot;
    private NighthawkAI _nighthawkAI;
    private NighthawkBombDrop _nighthawkBombDrop;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _playerMovement = GameObject.Find("/Player Manager/Player").GetComponent<PlayerMovement>();
        _enemyShoot = GetComponentInChildren<EnemyShoot>();
        _nighthawkAI = GetComponent<NighthawkAI>();
        _nighthawkBombDrop = GetComponentInChildren<NighthawkBombDrop>();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.isHardModeOn)
            {
                _movementSpeed *= 1.5f;
            }
            else
            {
                _movementSpeed *= 1.0f;
            }
        }

        _direction = Vector3.left;
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        Vector3 velocity = _direction * _movementSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        _isChangingDirection = true;

        if (_nighthawkAI != null)
        {
            _nighthawkAI.ResetVisibility();
        }

        if (_playerMovement != null && _isChangingDirection)
        {
            DetermineDirection();
        }
    }

    private void DetermineDirection()
    {
        _isChangingDirection = false;

        float xPos;
        bool bombDrop;

        if (_enemyShoot != null)
        {
            if (transform.position.x < 0)
            {
                xPos = -42.0f;
                _direction = Vector3.right;
                _anim.SetBool("FlyRight", true);
                _enemyShoot.UpdateLookDirection(Vector3.right);
            }
            else
            {
                xPos = 42.0f;
                _direction = Vector3.left;
                _anim.SetBool("FlyRight", false);
                _enemyShoot.UpdateLookDirection(Vector3.left);
            }

            if (_playerMovement.CurrentYPosition() < 9.0f)
            {
                _spawnPos = new Vector3(xPos, _playerMovement.CurrentYPosition() + 4.0f, 0.0f);
                transform.position = _spawnPos;
                _movementSpeed = 20.0f;
                bombDrop = true;
            }
            else
            {
                _spawnPos = new Vector3(xPos, _playerMovement.CurrentYPosition(), 0.0f);
                transform.position = _spawnPos;
                _movementSpeed = 40.0f;
                bombDrop = false;
            }

            _nighthawkBombDrop.enabled = bombDrop;
        }
    }
}
