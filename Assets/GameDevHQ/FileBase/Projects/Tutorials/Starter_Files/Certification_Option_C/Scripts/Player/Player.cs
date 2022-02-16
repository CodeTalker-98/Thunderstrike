using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Adjustable Values")]

    [Header("Debug Values")]
    [SerializeField] private bool _canEnableShield = false;
    [SerializeField] private bool _canEnableInvincibility = false;

    public void ChangeWeapon()
    {

    }

    public void EnableShield()
    {
        _canEnableShield = true;
    }

    public void EnableInvincibility()
    {
        _canEnableInvincibility = true;
    }
}
