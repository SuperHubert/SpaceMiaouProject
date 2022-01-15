using UnityEngine;

public class DamageOnTriggerStay : MonoBehaviour
{
    public int damage = 1;
    public bool canDamage = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!canDamage) return;
        LifeManager.Instance.TakeDamages(damage);
    }
    
}
