using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeppelinShoot : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _fireRate = 1.0f;
    [SerializeField] private Vector3[] _direction;

    [Header("Debug")]
    [SerializeField] private Stage _stage;

    [Header("Prefabs")]
    [SerializeField] private GameObject _projectile;
    [SerializeField] private GameObject _multiProjectile;
    [SerializeField] private GameObject _homingProjectile;
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _shootMissileSound;

    private enum Stage { Basic, Modified, Final}

    private float _cycleTime;

    private Player _player;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _player = GameObject.Find("/Player Manager/Player").GetComponent<Player>();

        if (GameManager.instance != null)
        {
            if (GameManager.instance.isHardModeOn)
            {
                _fireRate /= 2.0f;
            }
            else
            {
                _fireRate /= 1.0f;
            }
        }
    }

    private void Update()
    {
        SelectedStage();
    }

    private void SelectedStage()
    {
        switch (_stage)
        {
            case Stage.Basic:
                _fireRate = 2.0f;
                CalculateShot(_projectile, _fireRate);
                break;
            case Stage.Modified:
                _fireRate = 4.0f;
                if (_player != null)
                {
                    CalculateHomingShot(_homingProjectile, _fireRate);
                }
                break;
            case Stage.Final:
                _fireRate = 1.0f;
                CalculateShot(_multiProjectile, _fireRate);
                break;
            default:
                break;
        }
    }

    private void CalculateShot(GameObject projectilePrefab, float fireRate)
    {
        if (_cycleTime < Time.time)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            ProjectileMovement[] projectileMovement;

            if (projectile.transform.childCount < 1)
            {
               projectileMovement = projectile.GetComponents<ProjectileMovement>();
            }
            else
            {
                projectileMovement = projectile.GetComponentsInChildren<ProjectileMovement>();
            }
            
            if (projectileMovement != null)
            {
                for (int i = 0; i < projectileMovement.Length; i++)
                {
                    projectileMovement[i].SetProjectileDirection(_direction[i]);
                }
            }

            AudioManager.instance.PlaySound(_shootSound);

            _cycleTime = fireRate + Time.time;
        } 
    }

    private void CalculateHomingShot(GameObject projectilePrefab, float fireRate)
    {
        if (_cycleTime < Time.time)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            ProjectileMovement[] projectileMovement;

            if (projectile.transform.childCount < 1)
            {
                projectileMovement = projectile.GetComponents<ProjectileMovement>();
            }
            else
            {
                projectileMovement = projectile.GetComponentsInChildren<ProjectileMovement>();
            }

            if (projectileMovement != null)
            {
                for (int i = 0; i < projectileMovement.Length; i++)
                {
                    projectileMovement[i].SetProjectileDirection(_direction[i]);
                }
            }

            AudioManager.instance.PlaySound(_shootMissileSound);

            _cycleTime = fireRate + Time.time;
        }
    }

    public void BasicShoot()
    {
        _stage = Stage.Basic;
    }

    public void ModifiedShoot()
    {
        _stage = Stage.Modified;
    }

    public void FinalShoot()
    {
        _stage = Stage.Final;
    }
}
