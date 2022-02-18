using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _movementSpeed = 5.0f;

    private Vector3 _playerBounds = Vector3.zero;
    private Vector3 _moveDirection = Vector3.zero;

    public void CalculateMovement(Vector2 input)
    {
        _moveDirection = new Vector3(input.x, input.y, 0.0f);

        float xClamp = Mathf.Clamp(transform.position.x, -25.0f, 25.0f);
        float yClamp = Mathf.Clamp(transform.position.y, -15.0f, 13.0f);
        var velocity = _moveDirection * _movementSpeed;

        _playerBounds = new Vector3(xClamp, yClamp, 0.0f);
        transform.position = _playerBounds;

        transform.Translate(velocity * Time.deltaTime);
    }
}
