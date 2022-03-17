using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NighthawkAI : Enemy
{
    private bool _isInvisible = false;

    private GameObject _nighthawk;

    private Shader _invisibleShader;
    private Shader _baseShader;

    private Material _mat;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        _nighthawk = transform.GetChild(0).gameObject;

        _mat = _nighthawk.GetComponent<Renderer>().material;

        _invisibleShader = Shader.Find("Shader Graphs/NighthawkShader_asset");
        _baseShader = Shader.Find("Universal Render Pipeline/Lit");

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
            _mat.shader = _invisibleShader;
        }
        else
        {
            _mat.shader = _baseShader;
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
