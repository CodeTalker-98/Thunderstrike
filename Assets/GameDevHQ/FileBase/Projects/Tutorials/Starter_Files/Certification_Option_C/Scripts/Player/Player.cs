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
        CheckLevelComplete();
        //Debug.Log("Score: " + _score);
    }

    private void CheckLevelComplete()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.levelComplete)
            {
                GameManager.instance.GetScore(_score);
            }
        }
    }

    public void Damage(int damageAmount)
    {
        if (GameManager.instance.isDead || _canEnableInvincibility)
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
            _uiManager.DisplayWeaponName(Health);
        }

        if (Health < 1)
        {
            GameManager.instance.isDead = true;
            Instantiate(_deathPrefab, transform.position, Quaternion.identity);
            
            if (_score > -1)
            {
                UpdateScore((-_score / 2));
                GameManager.instance.GetScore(_score);
            }

            //play death sound

            if (GameManager.instance != null)
            {
                GameManager.instance.GameOverScreen();
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
            _uiManager.DisplayWeaponName(Health);
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
    }

    public void UpdateScore(int scoreValue)
    {
        _score += scoreValue;
        _uiManager.DisplayScore(_score);
    }

    private void OnEnable()
    {
        if(GameManager.instance != null)
        {
            if (GameManager.instance.isDead)
            {
                PlayerStatReset();
            }
        }

        UIReset();
    }

    private void PlayerStatReset()
    {
        GameManager.instance.isDead = false;
        transform.position = Vector3.zero;
        Health = _health;
        _canEnableShield = false;
        _canEnableInvincibility = false;
    }
}
