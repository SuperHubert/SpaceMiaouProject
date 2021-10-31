using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivationZone : MonoBehaviour
{
    private IEnemy enemy;
    private GameObject timeOutZone;
    
    private void Start()
    {
        enemy = transform.parent.parent.GetComponent<IEnemy>();
        timeOutZone = transform.parent.GetChild(2).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        enemy.Activate();
        timeOutZone.SetActive(true);
    }
}
