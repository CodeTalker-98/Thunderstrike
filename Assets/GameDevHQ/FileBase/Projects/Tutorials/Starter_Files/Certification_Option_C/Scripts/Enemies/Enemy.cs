using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public int Health { get; set; }

    public void Damage(int damageAmount)
    {
        
    }
}
