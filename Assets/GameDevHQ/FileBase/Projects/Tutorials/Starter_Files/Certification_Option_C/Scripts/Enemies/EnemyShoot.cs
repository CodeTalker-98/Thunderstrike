using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _lineOfSight = 50.0f;

    [Header("Prefabs")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firingOffset;

    private float _cycleTime = -1.0f;

    private void FixedUpdate()
    {
        ShootProjectile(_bulletPrefab);
    }

    private void ShootProjectile(GameObject projectile)
    {
        if (_cycleTime < Time.time)
        {
            RaycastHit hitInfo;
            Vector3 direction = Vector3.left;

            if (Physics.Raycast(transform.position, direction, out hitInfo, _lineOfSight))
            {
                if (hitInfo.collider.tag == "Player")
                {
                    GameObject firedProjectile = Instantiate(projectile, _firingOffset.position, Quaternion.identity);
                    ProjectileMovement[] projectiles = firedProjectile.GetComponents<ProjectileMovement>();

                    //Play sound

                    for (int i = 0; i < projectiles.Length; i++)
                    {
                        //Do something if need be
                    }

                    _cycleTime = Time.time + _fireRate;
                }
            }
        }
    }
}
