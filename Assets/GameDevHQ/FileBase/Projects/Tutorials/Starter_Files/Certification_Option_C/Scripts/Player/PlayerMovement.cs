using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _movementSpeed = 5.0f;

    private Vector3 _moveDirection = Vector3.zero;

    private void Start()
    {
        Init();
    }

    private void Init()
    {

    }

    public void CalculateMovement(Vector2 input)
    {
        _moveDirection = new Vector3(input.x, input.y, 0.0f);

        var velocity = _moveDirection * _movementSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }
}
