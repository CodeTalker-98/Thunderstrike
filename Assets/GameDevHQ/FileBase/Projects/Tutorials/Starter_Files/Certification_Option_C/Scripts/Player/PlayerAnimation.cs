using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Vector2 _movementInput = Vector2.zero;

    private Animator _anim;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void CalculateAnimation(Vector2 input)
    {
        float xInput = input.x;
        float yInput = input.y;

        _anim.SetFloat("X Value", xInput);
        _anim.SetFloat("Y Value", yInput);
    }
}
