using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [SerializeField] private int bulletDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        LifeManager.Instance.TakeDamages(bulletDamage);
        gameObject.SetActive(false);
    }
    
    public void SetBulletDamage(int damage)
    {
        bulletDamage = damage;
    }
}
