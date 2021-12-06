using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SprayAttack : MonoBehaviour
{
    public bool isSpraying;
    private bool canShoot = true;

    public Slider slider;
    public int maxSpray = 100;
    public float currentSpray;

    public float sprayAttackAxis;
    

    void Start()
    {
        currentSpray = maxSpray;
    }
    
    void Update()
    {
        SprayingAttack();
        //slider.value = currentSpray / maxSpray;
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
                bullet.GetComponent<BulletController>().direction = new Vector2(GetComponent<PlayerMovement>().lastDirection.x + Random.Range(-0.07f,0.07f),
                    GetComponent<PlayerMovement>().lastDirection.y +Random.Range(-0.07f,0.07f)).normalized;
                bullet.GetComponent<Rigidbody2D>().AddForce(bullet.GetComponent<BulletController>().direction 
                                                            * bullet.GetComponent<BulletController>().bulletForce,ForceMode2D.Impulse);
                currentSpray -= 1;

                canShoot = false;
                StartCoroutine(ResetSpray());
            }
        }

        else
        {
            isSpraying = false;
        }
    }

    IEnumerator ResetSpray()
    {
        yield return new WaitForSeconds(0.1f);
        isSpraying = false;
        canShoot = true;
    }
}
