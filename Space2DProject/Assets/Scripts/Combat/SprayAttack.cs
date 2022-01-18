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

    public Image sprayImage;
    public float maxSpray = 100;
    public float currentSpray;

    public bool burn = false;
    public float burnDamage = 0.02f;

    public List<Dialogues> sprayDialogues;

    [HideInInspector] public float sprayAttackAxis;

    private AudioManager am;
    

    void Start()
    {
        am = AudioManager.Instance;
        currentSpray = maxSpray;
    }
    
    void Update()
    {
        if(!InputManager.canInput) return;
        SprayingAttack();
    }

    void SprayingAttack()
    {
        if (sprayAttackAxis > 0 && GetComponent<Combat2>().isAttacking == false && GetComponent<Combat2>().isSpecialAttacking == false)
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
                
                am.Play(33);
                
                StartCoroutine(ResetSpray());
            }
            
            UpdateSprayBar();
        }

        else
        {
            isSpraying = false;
            if (!LoadingLevelData.sprayAmmoDialogue || !(currentSpray < 33)) return;
            DialogueManager.Instance.StartMultipleDialogues(sprayDialogues);
            LoadingLevelData.sprayAmmoDialogue = false;
        }
    }

    public void UpdateSprayBar()
    {
        if (currentSpray > maxSpray) currentSpray = maxSpray;
        sprayImage.fillAmount = (float)currentSpray / maxSpray;
    }

    IEnumerator ResetSpray()
    {
        yield return new WaitForSeconds(0.1f);
        isSpraying = false;
        canShoot = true;
    }
}
