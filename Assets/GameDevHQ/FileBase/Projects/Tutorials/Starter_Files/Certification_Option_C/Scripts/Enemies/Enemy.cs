using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    [Header("Adjustable Values")]
    [SerializeField] protected int _health = 1;
    [SerializeField] protected int _collisionDamage = 1;
    [SerializeField] protected int _scoreValue = 500;
    [SerializeField] protected float _colorChangeTime = 1.0f;
    [SerializeField] protected Color[] _colors;

    [Header("Debug")]
    [SerializeField] protected bool _canSpawnPrefab = false;
    [SerializeField] protected bool _isBoss = false;

    [Header("Prefabs")]
    [SerializeField] protected GameObject _powerupPrefab;
    [SerializeField] protected GameObject _collisionPrefab;
    [SerializeField] protected GameObject _deathPrefab;

    private int _colorIndex = 0;

    private float _colorCycleTime = -1.0f;

    private Player _player;

    private MeshRenderer _renderer;

    public int Health { get; set; }

    private void Awake()
    {
        _renderer = this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
        _player = GameObject.Find("/Player Manager/Player").GetComponent<Player>();
    }

    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.isHardModeOn)
            {
                Health = _health * 2;
                _scoreValue *= 2;
            }
            else
            {
                Health = _health;
                _scoreValue *= 1;
            }
        }

        int randomInt = Random.Range(0, 5); //CHGANGE TO 5!!!!

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
        _renderer.material.color = Color.Lerp(_renderer.material.color, _colors[_colorIndex], _colorChangeTime * Time.deltaTime);
        
        if (_colorCycleTime < Time.time)
        {
            _colorCycleTime = _colorChangeTime + Time.time;

            if (_colorIndex < _colors.Length - 1 )
            {
                _colorIndex ++;
            }
            else
            {
                _colorIndex = 0;
            }
        }
    }

    public virtual void Damage(int damageAmount)
    {
        Health -= damageAmount;

        if (Health < 1)
        {
            if (_player != null)
            {
                _player.UpdateScore(_scoreValue);
            }

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
                Instantiate(_collisionPrefab, other.transform.position, Quaternion.identity);
                Damage(_collisionDamage);             
                hit.Damage(_collisionDamage);
            }
        }
    }
}
