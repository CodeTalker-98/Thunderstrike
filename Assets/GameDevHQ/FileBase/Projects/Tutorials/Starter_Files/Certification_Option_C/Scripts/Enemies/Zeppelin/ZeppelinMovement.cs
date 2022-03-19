using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeppelinMovement : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _movementSpeed = 10.0f;

    [Header("Prefabs")]
    [SerializeField] private Transform _start;

    private bool _canMove = true;

    private Vector3 _target;

    private Player _player;

    private void Start()
    {
        Init();
        SetTargetPosition();
    }

    private void Init()
    {
        _player = GameObject.Find("/Player Manager/Player").GetComponent<Player>();
    }

    private void SetTargetPosition()
    {
        if (_player != null)
        {
            _target = _player.transform.position;
        }
        else
        {
            _target = Vector3.zero;
        }
    }

    public void BasicMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _start.position, Time.deltaTime * _movementSpeed);
    }

    public void ModifiedMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _movementSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, _target);

        if (distance < 1.0f)
        {
            SetTargetPosition();
        }
    }

    public void FinalMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _movementSpeed * 2 * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, _target);

        if (distance < 1.0f)
        {
            SetTargetPosition();
        }
    }
}
