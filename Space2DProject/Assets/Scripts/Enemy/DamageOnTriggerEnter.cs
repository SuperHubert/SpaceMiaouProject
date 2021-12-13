using UnityEngine;
public class DamageOnTriggerEnter : MonoBehaviour
{
    public int damage = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != 6) return;
        LifeManager.Instance.TakeDamages(damage);
    }
}
