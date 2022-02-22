using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("Adjustable Values")]
    [SerializeField] protected int _health = 1;
    [SerializeField] protected int _collisionDamage = 1;
    [SerializeField] protected int _scoreValue = 500;
    [SerializeField] protected float _colorChangeTime = 1.0f;

    [Header("Debug")]
    [SerializeField] protected bool _canSpawnPrefab = false;
    [SerializeField] protected bool _isBoss = false;

    [Header("Prefabs")]
    [SerializeField] protected GameObject _powerupPrefab;
    [SerializeField] protected GameObject _deathPrefab;

    private MeshRenderer _renderer;

    private Color _currentColor;
    private Color _targetColor;

    public int Health { get; set; }

    private void Awake()
    {
        _renderer = GameObject.Find("HIND Model").GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Health = _health;

        int randomInt = Random.Range(0, 1);

        if (randomInt == 0 && !_isBoss)
        {
            _canSpawnPrefab = true;
        }
    }

    private void Update()
    {
        if (_canSpawnPrefab && _renderer != null)
        {
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        if (_colorChangeTime < Time.time)
        {
            _currentColor = _targetColor;

            _renderer.material.color = _currentColor;

            _targetColor = new Color(Random.value, Random.value, Random.value);

            float time = _colorChangeTime;
            _colorChangeTime = time + Time.time;
        }
        else
        {
            _renderer.material.color = Color.Lerp(_currentColor, _targetColor, _colorChangeTime);

            //_colorChangeTime -= Time.deltaTime;
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

            Instantiate(_deathPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IDamagable hit = other.GetComponent<IDamagable>();

            if (hit != null)
            {
                Damage(_collisionDamage);
                hit.Damage(_collisionDamage);
            }
        }
    }
}
