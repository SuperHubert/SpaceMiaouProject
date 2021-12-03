using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallShooterBehaviour : EnemyBehaviour
{
    private GameObject bullet;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private int shootCdMax;
    [SerializeField] private int shootCd;
    void Update()
    {
        if(currentState != State.Awake) return;
        LookAt(player);
        if (!IsStopped()) return;
        if (isPerformingAction)
        {
            isPerformingAction = false;
        }
        else
        {
            if (shootCd > 0) 
            { 
                shootCd--;
            }
            else
            {
                ShootBullet();
                shootCd = shootCdMax;
            }
        }
    }
    
    public override void Action()
    {
        shootCd = shootCdMax;
        RunAway();
    }

    private void RunAway()
    {
        agent.SetDestination(enemyTransform.position + (enemyTransform.position - player.position).normalized * 1);
    }
    
    private void LookAt(Transform target)
    {
        Vector3 dir = target.position - enemyTransform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        enemyTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    private void ShootBullet()
    {
        bullet = ObjectPooler.Instance.SpawnFromPool("Enemy Bullets", enemyTransform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = (player.position - enemyTransform.position).normalized*bulletSpeed;
        bullet.GetComponent<BulletDamage>().SetBulletDamage(bulletDamage);
        bullet.layer = 11;
    }
    
    private bool IsStopped()
    {
        if (agent.pathPending) return false;
        if (!(agent.remainingDistance <= agent.stoppingDistance)) return false;
        return !agent.hasPath || agent.velocity.sqrMagnitude == 0f;
    }
}
