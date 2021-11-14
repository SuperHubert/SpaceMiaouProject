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
    
    private void LookAt(Transform target)
    {
        Vector3 dir = target.position - enemy.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        enemy.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    private void ShootBullet()
    {
        bullet = ObjectPooler.Instance.SpawnFromPool("Enemy Bullets", enemy.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = (player.position - enemy.position).normalized*bulletSpeed;
        ConsoleManager.Instance.Print(bullet.GetComponent<Rigidbody2D>().velocity.ToString());
        bullet.GetComponent<BulletDamage>().SetBulletDamage(bulletDamage);
        bullet.layer = 11;
    }
}
