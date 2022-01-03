using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private GameObject warningObj;
    [SerializeField] private GameObject rootObj;
    private CircleCollider2D circleCollider;
    private Animator animator;
    
    [SerializeField] private int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
        animator = GetComponentInChildren<Animator>();
    }

    public void Spawn()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        //play animation
        //wait for animation to finish
        //damage
        //play animation
        //die

        yield return new WaitForSeconds(1f);
        
        //warningObj.SetActive(false);
        rootObj.SetActive(true);
        animator.SetTrigger("Eat");

        circleCollider.enabled = true;
        
        
        
        yield return new WaitForSeconds(0.5f);
        
        //warningObj.SetActive(true);
        rootObj.SetActive(false);
        
        
        circleCollider.enabled = false;
        
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bonk");
        LifeManager.Instance.TakeDamages(damage);
    }
}
