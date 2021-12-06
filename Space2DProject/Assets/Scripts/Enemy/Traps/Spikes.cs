using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //play Animation
        LifeManager.Instance.TakeDamages(damage);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //play Animation
    }
}
