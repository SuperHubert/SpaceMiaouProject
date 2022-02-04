using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    public float damage = 10;
    [SerializeField] private bool specialAttack = false;
    private Combat3 combat;

    private void Start()
    {
        combat = transform.parent.GetComponent<Combat3>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        var enemy = other.gameObject;
        if (enemy.GetComponent<EnemyHealth>() != null)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(damage+combat.bonusDamage,true);
            combat.SprayGain(specialAttack);
            if (!specialAttack) combat.hitTrigger = true;
        }
        if(enemy.GetComponent<Chest>() != null) enemy.GetComponent<IInteractible>().OnInteraction();
        if(enemy.GetComponent<Colonne>() != null) enemy.GetComponent<IInteractible>().OnInteraction();
    }
}
