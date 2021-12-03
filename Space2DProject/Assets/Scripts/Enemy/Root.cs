using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private GameObject warningObj;
    [SerializeField] private GameObject rootObj;
    
    [SerializeField] private int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        //play animation
        //wait for animation to finish
        //damage
        //play animation
        //die
        
        yield return new WaitForSeconds(1f);
        
        warningObj.SetActive(false);
        rootObj.SetActive(true);
        
        yield return new WaitForSeconds(1f);
        
        Destroy(gameObject);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bonk");
        LifeManager.Instance.TakeDamages(damage);
    }
}
