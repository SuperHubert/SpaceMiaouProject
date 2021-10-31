using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerZone : MonoBehaviour
{
    private IEnemy enemy;
    
    private void Start()
    {
        enemy = transform.parent.parent.GetComponent<IEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        enemy.OnTriggerZoneEnter();
    }
}
