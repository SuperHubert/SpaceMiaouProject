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
    public bool onPlayer;

    private void Start()
    {
        cooldown = cooldownMax;
        col = GetComponent<Collider2D>();
        for (int i = 0; i < attacks.Count; i++)
        {
            attacks[i] = Instantiate(attacks[i], Vector3.zero, Quaternion.identity);
            attacks[i].SetActive(false);
        }
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
        foreach (var obj in attacks)
        {
            obj.SetActive(false);
        }
        var atq = attacks[Random.Range(0, attacks.Count)];
        if (onPlayer) atq.transform.position = LevelManager.Instance.Player().transform.position;
        atq.SetActive(true);
        cooldown = 0;
        col.enabled = false;
    }
}
