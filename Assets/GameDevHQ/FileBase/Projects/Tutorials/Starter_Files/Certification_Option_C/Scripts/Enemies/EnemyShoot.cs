using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _lineOfSight = 50.0f;
    [SerializeField] private LookDirection _lookDirection;

    [Header("Prefabs")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firingOffset;

    private bool _canUpdateLook = false;

    private float _cycleTime = -1.0f;

    private Vector3 _aimDirection;

    private enum LookDirection
    {
        Up,
        Down,
        Left,
        Right,
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
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
        if (!_canUpdateLook)
        {
            SetDirection();
        }
    }

    private void SetDirection()
    {
        switch (_lookDirection)
        {
            case LookDirection.Up:
                _aimDirection = Vector3.up;
                break;
            case LookDirection.Down:
                _aimDirection = Vector3.down;
                break;
            case LookDirection.Left:
                _aimDirection = Vector3.left;
                break;
            case LookDirection.Right:
                _aimDirection = Vector3.right;
                break;
            default:
                break;
        }
    }

    public void UpdateLookDirection(Vector3 lookDirection)
    {
        _canUpdateLook = true;
        _aimDirection = lookDirection;
    }

    private void FixedUpdate()
    {
        ShootProjectile(_bulletPrefab, _aimDirection);
    }

    private void ShootProjectile(GameObject projectile, Vector3 aimDirection)
    {
        if (_cycleTime < Time.time)
        {
            RaycastHit hitInfo;
            Vector3 direction = aimDirection;

            if (Physics.Raycast(transform.position, direction, out hitInfo, _lineOfSight))
            {
                if (hitInfo.collider.tag == "Player")
                {
                    GameObject firedProjectile = Instantiate(projectile, _firingOffset.position, Quaternion.identity);
                    ProjectileMovement[] projectiles = firedProjectile.GetComponents<ProjectileMovement>();

                    //Play sound

                    for (int i = 0; i < projectiles.Length; i++)
                    {
                        projectiles[i].SetProjectileDirection(direction);
                        //Do something if need be
                    }

                    _cycleTime = Time.time + _fireRate;
                }
            }
        }
    }
}
