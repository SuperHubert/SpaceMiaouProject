using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SprayAttack : MonoBehaviour
{
    public float damage = 3f;
    
    public bool isSpraying;
    private bool canShoot = true;

    public Slider slider;
    public float maxSpray = 100;
    public float currentSpray;

    public bool burn = false;
    public float burnDamage = 0.02f;
    
    [HideInInspector] public float sprayAttackAxis;
    

    void Start()
    {
        currentSpray = maxSpray;
    }
    
    void Update()
    {
        if(!InputManager.canInput) return;
        SprayingAttack();
    }

    void SprayingAttack()
    {
        if (sprayAttackAxis > 0 && GetComponent<Combat>().isAttacking == false &&
            GetComponent<Combat>().isSpecialAttacking == false)
        {
            isSpraying = true;
            if (canShoot && currentSpray > 0)
            {
                GameObject bullet = ObjectPooler.Instance.SpawnFromPool("Player Bullet", transform.position, transform.rotation);
                BulletController controller = bullet.GetComponent<BulletController>();
                controller.direction = new Vector2(GetComponent<PlayerMovement>().lastDirection.x + Random.Range(-0.07f,0.07f),
                    GetComponent<PlayerMovement>().lastDirection.y +Random.Range(-0.07f,0.07f)).normalized;
                bullet.GetComponent<Rigidbody2D>().AddForce(bullet.GetComponent<BulletController>().direction 
                                                            * bullet.GetComponent<BulletController>().bulletForce,ForceMode2D.Impulse);
                controller.damage = damage;
                controller.burn = burn;
                
                currentSpray -= 1;

                canShoot = false;
                StartCoroutine(ResetSpray());
            }
            
            UpdateSprayBar();
        }

        else
        {
            isSpraying = false;
        }
    }

    public void UpdateSprayBar()
    {
        slider.value = currentSpray / maxSpray;
    }

    IEnumerator ResetSpray()
    {
        yield return new WaitForSeconds(0.1f);
        isSpraying = false;
        canShoot = true;
    }
}
