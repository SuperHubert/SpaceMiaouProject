using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    private enum Trigger {WakeUp, Sleep, Respawn, Action}

    [SerializeField] private Trigger state;
    private EnemyBehaviour enemy;

    private void Start()
    {
        enemy = transform.parent.parent.gameObject.GetComponent<EnemyBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (state)
        {
            case Trigger.WakeUp:
                enemy.WakeUp();
                break;
            case Trigger.Action:
                enemy.ExecuteAction();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (state)
        {
            case Trigger.Sleep:
                enemy.Sleep();
                break;
            case Trigger.Respawn:
                enemy.Respawn();
                break;

        }
    }
}
