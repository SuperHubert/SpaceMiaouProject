using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStartAttack : MonoBehaviour
{
    private IEnemy enemy;
    
    private void Start()
    {
        enemy = transform.parent.GetComponent<IEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        enemy.OnTriggerZoneEnter();
    }
}
