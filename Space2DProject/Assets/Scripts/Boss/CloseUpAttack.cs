using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpAttack : MonoBehaviour
{
    public int damage = 1;
    public bool canDamage = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!canDamage) return;
        LifeManager.Instance.TakeDamages(damage);
        canDamage = false;
    }
}
