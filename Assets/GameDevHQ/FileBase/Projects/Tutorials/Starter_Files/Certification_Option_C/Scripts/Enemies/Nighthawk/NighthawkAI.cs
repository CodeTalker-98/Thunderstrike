using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NighthawkAI : Enemy
{
    private bool _isInvisible = false;

    private GameObject _nighthawk;

    [SerializeField] private Material _invisibleShader;
    [SerializeField] private Material _baseShader;

    private MeshRenderer _mesh;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        _nighthawk = transform.GetChild(0).gameObject;

        _mesh = _nighthawk.GetComponent<MeshRenderer>();

        _isInvisible = true;
    }

    private void Update()
    {
        Visibility();
    }

    private void Visibility()
    {
        if (_isInvisible)
        {
            _mesh.material = _invisibleShader;
        }
        else
        {
            _mesh.material = _baseShader;
        }

    }

    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);
        _isInvisible = false;
    }

    public void ResetVisibility()
    {
        _isInvisible = true;
    }
}
