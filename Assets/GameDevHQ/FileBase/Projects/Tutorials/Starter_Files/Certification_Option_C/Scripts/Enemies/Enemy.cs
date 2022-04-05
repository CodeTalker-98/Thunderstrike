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
    [SerializeField] protected float _xStart = 45.0f;
    [SerializeField] protected float _yStart = 0.0f;

    [Header("Debug")]
    [SerializeField] protected bool _canSpawnPrefab = false;
    [SerializeField] protected bool _isBoss = false;
    [SerializeField] protected bool _isBomber = false;
    [SerializeField] protected bool _cantRandomize = false;
    [SerializeField] protected bool _childMoves = false;

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

        if (!_isBoss)
        {
            if (_cantRandomize)
            {
                _yStart = transform.position.y;
            }
            else
            {
                if (_isBomber)
                {
                    _yStart = Random.Range(6.0f, 12.0f);
                }
                else
                {
                    _yStart = Random.Range(-15.0f, 12.0f);
                }
            }

            transform.position = new Vector3(_xStart, _yStart, 0.0f);

        }
        else
        {
            transform.position = new Vector3(_xStart, 0.0f, 0.0f);
        }

        int randomInt = Random.Range(0, 8);

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
            _player = GameObject.Find("/Player Manager/Player").GetComponent<Player>();

            if (_player != null)
            {
                _player.UpdateScore(_scoreValue);
            }

            if (_canSpawnPrefab)
            {
                if (!_childMoves)
                {
                    Instantiate(_powerupPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    Transform childPosition = this.gameObject.transform.GetChild(0).transform;

                    Instantiate(_powerupPrefab, childPosition.position, Quaternion.identity);
                }
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
