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
    //Negative score for deathv

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
        //update ui score, health, weapon
    }

    private void Init()
    {
        Health = _health;
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
            //update ui
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
        if (Health < 5)
        {
            Health ++;
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

        //update ui
    }
}
