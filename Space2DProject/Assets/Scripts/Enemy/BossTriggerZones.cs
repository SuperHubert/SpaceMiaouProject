using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class BossTriggerZones : MonoBehaviour
{
    public List<GameObject> attacks;
    public int cooldownMax;
    [SerializeField] private int cooldown;
    private Collider2D col;

    private void Start()
    {
        cooldown = cooldownMax;
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (cooldown < cooldownMax)
        {
            cooldown++;
        }
        else
        {
            cooldown = cooldownMax;
            col.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(cooldown != cooldownMax) return;
        var atq = Instantiate(attacks[Random.Range(0,attacks.Count)],Vector3.zero,Quaternion.identity);
        atq.SetActive(true);
        cooldown = 0;
        col.enabled = false;
    }
}
