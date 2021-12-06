using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private GameObject warningObj;
    [SerializeField] private GameObject rootObj;
    private CircleCollider2D circleCollider;
    
    [SerializeField] private int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
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
        
        warningObj.SetActive(false);
        rootObj.SetActive(true);

        circleCollider.enabled = true;
        
        
        yield return new WaitForSeconds(1f);
        
        warningObj.SetActive(true);
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
