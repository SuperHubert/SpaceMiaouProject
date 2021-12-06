using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomAttack : MonoBehaviour
{


    //Aim
    public GameObject detector;
    private List<Vector2> closeEnemies;
    private float attackAngle;
    private float minAngle;
    public float autoAimField = 90;
    private Vector2 closestDirection;

    //Enemy
    public List<GameObject> enemiesHit;

    //Damage
    public int attackDamage = 2;


    private void Update()
    {
         Attack();
    }

    private List<Vector2> GetClosestEnemies()
    {
        List<Vector2> directionCloseEnemies = new List<Vector2>();
        foreach (GameObject enemy in detector.GetComponent<EnemyDetector>().closeEnemies)
        {
            directionCloseEnemies.Add((new Vector2(enemy.transform.position.x - transform.position.x,
                enemy.transform.position.y - transform.position.y)));
        }

        return directionCloseEnemies;
    }

    void Attack()
    {
        closeEnemies = GetClosestEnemies();
        if (closeEnemies.Count == 0)
        {
            attackAngle = Mathf.Atan2(GetComponentInParent<PlayerMovement>().lastDirection.y,
                GetComponentInParent<PlayerMovement>().lastDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, attackAngle);
        }
        else
        {
            minAngle = 90;

            foreach (Vector2 enemyDirection in closeEnemies)
            {
                attackAngle = Vector2.Angle(new Vector2(enemyDirection.x, enemyDirection.y),
                    GetComponentInParent<PlayerMovement>().lastDirection);

                if (minAngle > attackAngle)
                {
                    minAngle = attackAngle;
                    closestDirection = enemyDirection;
                }
            }

            if (minAngle < autoAimField)
            {
                transform.rotation = Quaternion.Euler(0, 0,
                    Mathf.Atan2(closestDirection.y, closestDirection.x) * Mathf.Rad2Deg);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0,
                    Mathf.Atan2(GetComponentInParent<PlayerMovement>().lastDirection.y,
                        GetComponentInParent<PlayerMovement>().lastDirection.x) * Mathf.Rad2Deg);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesHit.Add(other.transform.gameObject); 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesHit.Remove(other.transform.gameObject); 
        }
    }

    public void dealDamage()
    {
        foreach (GameObject enemy in enemiesHit)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);            
        }
    }
    
}