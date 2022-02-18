using System.Collections;
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

    public int Health { get; set; }

    private void Awake()
    {
        
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Health = _health;
    }

    private void Update()
    {
        Health = _health;
        Debug.Log("Health: " + Health);
    }

    public void Damage(int damageAmount)
    {
        if (_isDead)
        {
            return;
        }

        if (!_canEnableShield || !_canEnableInvincibility)
        {
            Health -= damageAmount;
        }

        if (Health < 1)
        {
            _isDead = true;
            //Instantiate death animation
            this.gameObject.SetActive(false);
        }

        if (_canEnableShield)
        {
            _canEnableShield = false;
        }
    }

    public void ChangeWeapon()
    {
        if (Health < 6)
        {
            Health ++;
        }
    }

    public void EnableShield()
    {
        _canEnableShield = true;
        _shieldPrefab.SetActive(true);
    }

    public void EnableInvincibility()
    {
        _canEnableInvincibility = true;
        _invincibilityPrefab.SetActive(true);
        //Do something with a shader to make it like Mario Kart Invincibility??
    }
}
