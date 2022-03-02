﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    [Header("Adjustable Values")]
    [SerializeField] private int _health = 1;

    [Header("Debug Values")]
    [SerializeField] private bool _canEnableShield = false;
    [SerializeField] private bool _canEnableInvincibility = false;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private int _score = 0;

    [Header("Prefabs")]
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _invincibilityPrefab;
    [SerializeField] private GameObject _deathPrefab;

    private int _maxHealth = 5;

    private UIManager _uiManager;

    public int Health { get; set; }

    private void Awake()
    {
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    private void Start()
    {
        Init();
        UIReset();
    }

    private void Init()
    {
        Health = _health;
    }

    private void UIReset()
    {
        _uiManager.DisplayScore(_score);
        _uiManager.DisplayHealth(Health, _maxHealth);
        _uiManager.DisplayWeaponName(Health);
    }

    private void Update()
    {
        Debug.Log("Health: " + Health);
        Debug.Log("Score: " + _score);
    }

    public void Damage(int damageAmount)
    {
        if (_isDead || _canEnableInvincibility)
        {
            return;
        }

        if (_canEnableShield)
        {
            _canEnableShield = false;
            _shieldPrefab.SetActive(false);
        }
        else
        {
            Health -= damageAmount;
            _uiManager.DisplayHealth(Health, _maxHealth);
        }

        if (Health < 1)
        {
            _isDead = true;
            Instantiate(_deathPrefab, transform.position, Quaternion.identity);
            
            if (_score > -1)
            {
                UpdateScore((-_score / 2));
            }

            this.gameObject.SetActive(false);
        }
    }

    public void ChangeWeapon()
    {
        if (Health < _maxHealth)
        {
            Health ++;
            _uiManager.DisplayHealth(Health, _maxHealth);
        }
    }

    public void EnableShield()
    {
        _canEnableShield = true;

        if (!_invincibilityPrefab.activeSelf)
        {
            _shieldPrefab.SetActive(true);
        }
    }

    public void EnableInvincibility()
    {
        _canEnableInvincibility = true;
        _shieldPrefab.SetActive(false);
        _invincibilityPrefab.SetActive(true);
        //Do something with a shader to make it like Mario Kart Invincibility??
    }

    public void UpdateScore(int scoreValue)
    {
        _score += scoreValue;
        _uiManager.DisplayScore(_score);
    }

    private void OnEnable()
    {
        //reset health, score, bools
        //reset ui score, health, weapon
        //Reset Wave #???
    }

    private void PlayerStatReset()
    {
        //reset scores and bools, health too Health = _health
    }
}
