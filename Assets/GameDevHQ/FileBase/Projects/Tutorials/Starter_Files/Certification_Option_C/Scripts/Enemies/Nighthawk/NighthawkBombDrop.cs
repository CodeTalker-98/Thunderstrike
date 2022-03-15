using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NighthawkBombDrop : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _fireRate = 0.5f;

    [Header("Prefabs")]
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private Transform _bombFiringPosition;

    private float _cycletime = -1.0f;

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
        StartBombRun();
    }

    private void StartBombRun()
    {
        if (_cycletime < Time.time)
        {
            GameObject bomb = Instantiate(_bombPrefab, _bombFiringPosition.position, Quaternion.identity);
            ProjectileMovement[] projectiles = bomb.GetComponents<ProjectileMovement>();

            for (int i = 0; i < projectiles.Length; i++)
            {
                projectiles[i].SetProjectileDirection(Vector3.down);
            }

            _cycletime = _fireRate + Time.time;
        }
    }
}
