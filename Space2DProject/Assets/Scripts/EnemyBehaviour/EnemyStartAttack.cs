using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStartAttack : MonoBehaviour
{
    private SmallEnemyBehaviour enemy;
    
    private void Start()
    {
        enemy = transform.parent.GetComponent<SmallEnemyBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        enemy.StartAttack();
    }
}
