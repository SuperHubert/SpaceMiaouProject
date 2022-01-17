using System;
using UnityEngine;
public class DamageOnTriggerEnter : MonoBehaviour
{
    public int damage = 1;
    public bool feetHitbox = false;
    public float detectOffset;
    public float moveOffset;
    private Transform playerTransform;
    private BoxCollider2D boxCollider;
    private float yPos;

    private void Start()
    {
        playerTransform = LevelManager.Instance.Player().transform;
        if (gameObject.GetComponent<BoxCollider2D>() != null)
        {
            boxCollider = gameObject.GetComponent<BoxCollider2D>();
            yPos = boxCollider.offset.y;
        }
        
    }

    private void Update()
    {
        if(!feetHitbox || boxCollider == null) return;
        if (playerTransform.position.y < transform.position.y + detectOffset)
        {
            boxCollider.offset = Vector2.right * yPos + Vector2.up * moveOffset;
        }
        else
        {
            boxCollider.offset = Vector2.right * yPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != 6) return;
        LifeManager.Instance.TakeDamages(damage);
    }
    
    
    
}
