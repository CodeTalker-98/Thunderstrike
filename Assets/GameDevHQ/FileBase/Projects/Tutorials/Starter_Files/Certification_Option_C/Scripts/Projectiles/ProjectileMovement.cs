using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _movementSpeed = 15.0f;
    [SerializeField] private float _homingTimer = 5.0f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private bool _isHomingProjectile = false;

    [Header("Prefabs")]
    [SerializeField] private GameObject _impactPrefab;

    private bool _isPlayerProjectile = false;

    private Vector3 _direction = Vector3.zero;

    private GameObject _target;

    private WaitForSeconds _homingMissileTimer;

    private void Start()
    {
        CheckID();
        Init();
    }

    private void Init()
    {       
        if (GameManager.instance != null)
        {
            if (GameManager.instance.isHardModeOn && !_isPlayerProjectile)
            {
                _damageAmount *= 2;
            }
            else
            {
                _damageAmount *= 1;
            }
        }
    }

    private void CheckID()
    {
        if (_isPlayerProjectile)
        {
            _direction = Vector3.right;
        }

        if (_isHomingProjectile)
        {
            _target = GameObject.Find("/Player Manager/Player");

            _homingMissileTimer = new WaitForSeconds(_homingTimer);

            if (_target != null)
            {
                StartCoroutine(HomingMissileTimer());
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Update()
    {
        if (!_isHomingProjectile)
        {
            CalculateMovement(_direction);
        }
        else
        {
            if (_target != null)
            {
                CalculateHomingMovement();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void CalculateMovement(Vector3 direction)
    {
        Vector3 velocity = direction * _movementSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }

    private void CalculateHomingMovement()
    {
        if (_target != null)
        {
            Vector3 direction = (_target.transform.position - transform.position).normalized;

            Vector3 rotateValue = Vector3.Cross(direction, -transform.right);

            Vector3 velocity = -transform.right * _movementSpeed;

            transform.Rotate(-rotateValue);
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _movementSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && _isPlayerProjectile || other.tag == "Player" && !_isPlayerProjectile)
        {
            IDamagable hit = other.GetComponent<IDamagable>();

            if (hit != null)
            {
                CallDamage(hit);
            }
            else
            {
                IDamagable hitParent = other.transform.parent.GetComponent<IDamagable>();

                if (hitParent != null)
                {
                    CallDamage(hitParent);
                }
            }
        }
    }

    private void CallDamage(IDamagable hit)
    {
        hit.Damage(_damageAmount);
        Instantiate(_impactPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
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

    IEnumerator HomingMissileTimer()
    {
        yield return _homingMissileTimer;
        Instantiate(_impactPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
