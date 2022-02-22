using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _movementSpeed = 15.0f;
    [SerializeField] private int _damageAmount = 1;

    [Header("Prefabs")]
    [SerializeField] private GameObject _impactPrefab;

    private bool _isPlayerProjectile = false;

    private Vector3 _direction = Vector3.zero;

    private SpriteRenderer _projectileRenderer;

    private void Start()
    {
        Init();
        CheckID();
    }

    private void Init()
    {
        _projectileRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void CheckID()
    {
        if (_isPlayerProjectile)
        {
            _direction = Vector3.right;
        }
        else
        {
            //_direction = Vector3.left;
            _projectileRenderer.flipX = true;
        }
    }

    private void Update()
    {
        CalculateMovement(_direction);
    }

    private void CalculateMovement(Vector3 direction)
    {
        Vector3 velocity = direction * _movementSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && _isPlayerProjectile || other.tag == "Player" && !_isPlayerProjectile)
        {
            IDamagable hit = other.GetComponent<IDamagable>();

            if (hit != null)
            {
                hit.Damage(_damageAmount);
                Instantiate(_impactPrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    public void SetPlayerID()
    {
        _isPlayerProjectile = true;
    }

    public void SetProjectileDirection(Vector3 direction)
    {
        _direction = direction;
    }
}
