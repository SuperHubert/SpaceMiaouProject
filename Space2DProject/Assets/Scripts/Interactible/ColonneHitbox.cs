using UnityEngine;

public class ColonneHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != 7) return;
        
        other.GetComponent<EnemyHealth>().TakeDamage(999);
    }
}
