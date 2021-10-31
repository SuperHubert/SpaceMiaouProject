using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] private int maxHp = 2;
    [SerializeField] private int currentHp;

    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Debug.Log("Enemy Killed");
            Destroy(gameObject);
        }
    }
}
