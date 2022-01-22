using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    public float damage = 10;
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.gameObject;
        enemy.GetComponent<EnemyHealth>().TakeDamage(damage,true);
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
