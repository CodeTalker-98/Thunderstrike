using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _movementSpeed = 15.0f;

    private bool _isPlayerProjectile = false;

    private Vector3 _direction = Vector3.zero;

    private void Start()
    {
        CheckID();
    }

    private void CheckID()
    {
        if (_isPlayerProjectile)
        {
            _direction = Vector3.right;
        }
        else
        {
            _direction = Vector3.left;
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

    public void SetPlayerID()
    {
        _isPlayerProjectile = true;
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
