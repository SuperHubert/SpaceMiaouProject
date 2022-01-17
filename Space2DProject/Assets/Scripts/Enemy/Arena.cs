using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public Transform level;
    private List<Animator> doors = new List<Animator>();
    private List<Transform> enemies = new List<Transform>();
    private bool activated = false;
    public bool completed = false;

    private void Start()
    {
        InitObjs();
    }

    void Update()
    {
        if(completed) return;
        if(!activated) return;
        if (!CheckIfKidsAreDead()) return;
        completed = true;
        OpenDoors();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("bonk");
        activated = true;
        SpawnEnemies();
        CloseDoors();
    }

    private void InitObjs()
    {
        foreach (Transform enemy in transform.GetChild(1))
        {
            enemies.Add(enemy);
        }
        
        foreach (Transform door in transform.GetChild(0))
        {
            if(door.gameObject.activeSelf) doors.Add(door.GetComponent<Animator>());
        }


        Transform parentHp;
        Transform parentEnemy;
        if (level != null)
        {
            parentHp = level.GetChild(5);
            parentEnemy = level.GetChild(1);
        }
        else
        {
            parentHp = LevelManager.Instance.Level().GetChild(5);
            parentEnemy = LevelManager.Instance.Level().GetChild(1);
        }
        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
            var hpBar = enemy.GetChild(0).GetComponent<EnemyHealth>().healthBarTransform;
            hpBar.SetParent(parentHp);
            hpBar.gameObject.SetActive(false);
            enemy.SetParent(parentEnemy);
        }
        OpenDoors();
    }
    
    private bool CheckIfKidsAreDead()
    {
        bool returnValue = true;
        foreach (var enemy in enemies.Where(enemy => enemy.GetChild(0).gameObject.activeSelf))
        {
            returnValue = false;
        }

        return returnValue;
    }

    private void SpawnEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    private void CloseDoors()
    {
        foreach (Transform door in transform.GetChild(0))
        {
            if (door.GetComponent<Collider2D>()!= null) door.GetComponent<Collider2D>().enabled = true;
        }
        foreach (var animator in doors)
        {
            animator.SetBool("Opened",false);
        }
    }
    
    private void OpenDoors()
    {
        Debug.Log("bonk");
        foreach (Transform door in transform.GetChild(0))
        {
            if (door.GetComponent<Collider2D>()!= null) door.GetComponent<Collider2D>().enabled = false;
        }

        foreach (var animator in doors)
        {
            animator.SetBool("Opened",true);
        }
    }
}
