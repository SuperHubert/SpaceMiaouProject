using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [SerializeField] private int bulletDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.layer)
        {
            case 13:
                return;
            case 6:
                LifeManager.Instance.TakeDamages(bulletDamage);
                break;
            default:
                break;
        }

        gameObject.SetActive(false);
    }
    
    public void SetBulletDamage(int damage)
    {
        bulletDamage = damage;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.SetActive(false);
    }
}
