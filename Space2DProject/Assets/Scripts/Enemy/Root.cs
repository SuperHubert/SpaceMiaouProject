using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private GameObject warningObj;
    [SerializeField] private GameObject rootObj;
    private CircleCollider2D circleCollider;
    public Animator animator;
    
    [SerializeField] private int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
    }

    public void Spawn()
    {
        animator.Rebind();
        animator.Update(0f);
        UpdateAppearance();
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        LifeManager.Instance.TakeDamages(damage);
    }
    
    
    private void UpdateAppearance()
    {
        var biome = LevelManager.Instance.GetBiome();
        
        switch (biome)
        {
            case 0:
                animator.SetLayerWeight (1, 1f);
                animator.SetLayerWeight (2, 0f);
                animator.SetLayerWeight (3, 0f);
                break;
            case 1:
                animator.SetLayerWeight (1, 0f);
                animator.SetLayerWeight (2, 1f);
                animator.SetLayerWeight (3, 0f);
                break;
            case 2:
                animator.SetLayerWeight (1, 0f);
                animator.SetLayerWeight (2, 0f);
                animator.SetLayerWeight (3, 1f);
                break;
            default:
                animator.SetLayerWeight (1, 1f);
                animator.SetLayerWeight (2, 0f);
                animator.SetLayerWeight (3, 0f);
                break;
        }

    }
}
