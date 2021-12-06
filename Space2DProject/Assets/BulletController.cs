using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletForce = 2f;
    public Vector2 direction;

    public int damage = 3;
    
    void OnEnable()
    {
        Invoke("Destroy", 0.8f);
    }
    
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.SetActive(false);
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
