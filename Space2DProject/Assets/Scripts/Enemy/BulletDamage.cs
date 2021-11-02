using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [SerializeField] private int bulletDamage;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        LifeManager.Instance.TakeDamages(bulletDamage);
        gameObject.SetActive(false);
    }

    public void SetBulletDamage(int damage)
    {
        bulletDamage = damage;
    }
}
