using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("Adjustable Values")]
    [SerializeField] protected int _health = 1;
    [SerializeField] protected int _scoreValue = 500;

    [Header("Debug")]
    [SerializeField] protected bool _canSpawnPrefab = false;
    [SerializeField] protected bool _isBoss = false;

    [Header("Prefabs")]
    [SerializeField] protected GameObject _powerupPrefab;
    [SerializeField] protected GameObject _deathPrefab;

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

        int randomInt = Random.Range(0, 5);

        if (randomInt == 0 && !_isBoss)
        {
            _canSpawnPrefab = true;
        }
    }

    public void Damage(int damageAmount)
    {
        Health -= damageAmount;

        if (Health < 1)
        {
            //Update Player score

            if (_canSpawnPrefab)
            {
                Instantiate(_powerupPrefab, transform.position, Quaternion.identity);
            }

            //Instantiate death
            Destroy(this.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
