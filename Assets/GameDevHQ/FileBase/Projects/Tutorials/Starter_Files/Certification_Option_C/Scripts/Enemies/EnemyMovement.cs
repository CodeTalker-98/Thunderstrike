using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _movementSpeed = 5.0f;

    private Vector3 _direction = Vector3.zero;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _direction = Vector3.left;
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        Vector3 velocity = _direction * _movementSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }
}
