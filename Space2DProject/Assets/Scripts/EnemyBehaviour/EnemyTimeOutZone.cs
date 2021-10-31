using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTimeOutZone : MonoBehaviour
{
    private IEnemy enemy;

    private void Start()
    {
        enemy = transform.parent.parent.GetComponent<IEnemy>();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        enemy.DeActivate();
        gameObject.SetActive(false);
    }
}
