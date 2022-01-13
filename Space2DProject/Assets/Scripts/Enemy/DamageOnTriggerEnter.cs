using System;
using UnityEngine;
public class DamageOnTriggerEnter : MonoBehaviour
{
    public int damage = 1;
    public bool feetHitbox = false;
    public float hitboxMove;
    public float detectOffset;
    public float moveOffset;
    private Transform playerTransform;
    private BoxCollider2D collider;
    private float yPos;

    private void Start()
    {
        playerTransform = LevelManager.Instance.Player().transform;
        if (gameObject.GetComponent<BoxCollider2D>() != null)
        {
            collider = gameObject.GetComponent<BoxCollider2D>();
            yPos = collider.offset.y;
        }
        
    }

    private void Update()
    {
        if(!feetHitbox || collider == null) return;
        if (playerTransform.position.y < transform.position.y + detectOffset)
        {
            collider.offset = Vector2.right * yPos + Vector2.up * moveOffset;
        }
        else
        {
            collider.offset = Vector2.right * yPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != 6) return;
        LifeManager.Instance.TakeDamages(damage);
    }
    
    
    
}
