using UnityEngine;

public class DamageOnTriggerStay : MonoBehaviour
{
    public int damage = 1;
    public bool canDamage = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!canDamage) return;
        LifeManager.Instance.TakeDamages(damage);
        canDamage = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!canDamage) return;
        LifeManager.Instance.TakeDamages(damage);
        canDamage = false;
    }
}
