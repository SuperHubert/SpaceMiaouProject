using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletForce = 2f;
    public Vector2 direction;

    public float damage = 3;

    public bool burn;
    public float burnDamage = 0.02f;

    public Animator animator;

    void OnEnable()
    {
        Invoke(nameof(Destroy), 0.8f);
    }
    
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != 7) return;
        
        other.GetComponent<EnemyHealth>().TakeDamage(damage);
        gameObject.SetActive(false);
        if(!burn) return;
        
        other.GetComponent<EnemyHealth>().Burn(burnDamage);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.SetActive(false);
    }

    private void Destroy()
    {
        //animator.Play("PopBulles");
        gameObject.SetActive(false);
    }
}
