using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeppelinAI : Enemy
{
    [Header("VFX Prefabs")]
    [SerializeField] private GameObject _stageTwoDamaagePrefab;
    [SerializeField] private GameObject _stageThreeDamaagePrefab;

    [Header("Boss Debug")]
    [SerializeField] private Stage _stage;

    private bool _secondStage;
    private bool _thirdStage;

    private int _maxHealth;

    private ZeppelinMovement _zeppelinMovement;

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
        _maxHealth = Health;
        _stage = Stage.Basic;
    }

    private void Update()
    {
        SetStage();

        Debug.Log("Stage: " + _stage);
    }

    private void SetStage()
    {
        if (_zeppelinMovement != null)
        {
            switch (_stage)
            {
                case Stage.Basic:
                    _zeppelinMovement.BasicMovement();
                    //Shoot phase 1
                    break;
                case Stage.Modified:
                    _zeppelinMovement.ModifiedMovement();
                    //Instantiate damage 1
                    //shoot phase 2
                    break;
                case Stage.Final:
                    _zeppelinMovement.FinalMovement();
                    //Instantiate damage 2
                    //shoot phase 3
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
        else if (Health < (_maxHealth * 0.66f) && Health >= (_maxHealth * 0.33f) /*&& _secondStage*/)
        {
            _stage = Stage.Modified;
        }
        else if (Health < (_maxHealth * 0.33f) /*&& _thirdStage*/)
        {
            _stage = Stage.Final;
        }
    }
}
