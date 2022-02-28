using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _lineOfSight = 50.0f;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] _bulletPrefab;
    [SerializeField] private Transform _firingOffset;

    [Header("Sounds")]
    [SerializeField] private AudioClip[] _gunSounds;

    private float _cycleTime = -1.0f;

    private int _prefabIndex = 1;

    private void FixedUpdate()
    {
        ShootProjectile(_bulletPrefab[(_prefabIndex - 1)]);
    }

    private void ShootProjectile(GameObject projectile)
    {
        if (_cycleTime < Time.time)
        {
            RaycastHit hitInfo;
            Vector3 direction = Vector3.right;

            if (Physics.Raycast(transform.position, direction, out hitInfo, _lineOfSight))
            {
                if (hitInfo.collider.tag == "Enemy")
                {
                    GameObject firedProjectile = Instantiate(projectile, _firingOffset.position, Quaternion.identity);
                    ProjectileMovement[] projectiles = firedProjectile.GetComponents<ProjectileMovement>();

                    if (projectiles != null)
                    {
                        for (int i = 0; i < projectiles.Length; i++)
                        {
                            projectiles[i].SetPlayerID();
                        }
                    }

                    AudioManager.instance.PlaySound(_gunSounds[_prefabIndex - 1], transform.position);

                    _cycleTime = Time.time + _fireRate;
                }
            }
        }
    }
}
