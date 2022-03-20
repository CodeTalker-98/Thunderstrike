using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeppelinAI : Enemy
{
    [Header("VFX Prefabs")]
    [SerializeField] private GameObject _stageTwoDamagePrefab;
    [SerializeField] private GameObject _stageThreeDamagePrefab;

    [Header("Boss Debug")]
    [SerializeField] private Stage _stage;

    private int _maxHealth;

    private CapsuleCollider _capsuleCollider;

    private ZeppelinMovement _zeppelinMovement;
    private ZeppelinShoot _zeppelinShoot;

    private enum Stage
    {
        Basic,
        Modified,
        Final
    }

    public override void Init()
    {
        base.Init();
        _zeppelinMovement = GetComponent<ZeppelinMovement>();
        _zeppelinShoot = GetComponentInChildren<ZeppelinShoot>();
        _maxHealth = Health;
        _stage = Stage.Basic;
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _capsuleCollider.enabled = false;
    }

    private void Update()
    {
        SetStage();
    }

    private void SetStage()
    {
        if (_zeppelinMovement != null && _zeppelinShoot != null)
        {
            switch (_stage)
            {
                case Stage.Basic:
                    _zeppelinMovement.BasicMovement();
                    _zeppelinShoot.BasicShoot();
                    break;
                case Stage.Modified:
                    _zeppelinMovement.ModifiedMovement();
                    _stageTwoDamagePrefab.SetActive(true);
                    _capsuleCollider.enabled = true;
                    _zeppelinShoot.ModifiedShoot();
                    break;
                case Stage.Final:
                    _zeppelinMovement.FinalMovement();
                    _stageThreeDamagePrefab.SetActive(true);
                    _zeppelinShoot.FinalShoot();
                    break;
                default:
                    break;
            }
        }
    }

    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);

        if (Health >= (_maxHealth * 0.66f))
        {
            return;
        }
        else if (Health < (_maxHealth * 0.66f) && Health >= (_maxHealth * 0.33f))
        {
            _stage = Stage.Modified;
        }
        else if (Health < (_maxHealth * 0.33f))
        {
            _stage = Stage.Final;
        }
    }
}
