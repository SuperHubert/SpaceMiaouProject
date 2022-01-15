using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public Transform level;
    public List<Animator> doors;
    private List<Transform> enemies;
    private bool activated = false;

    private void Start()
    {
        InitObjs();
    }

    void Update()
    {
        if (!CheckIfKidsAreDead() || !activated) return;
        OpenDoors();
        gameObject.SetActive(false);
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
        
        //var parentHp = LevelManager.Instance.Level().GetChild(5);
        //var parentEnemy = LevelManager.Instance.Level().GetChild(1);
        var parentHp = level.GetChild(5);
        var parentEnemy = level.GetChild(1);
        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
            enemy.GetChild(0).GetComponent<EnemyHealth>().healthBarTransform.SetParent(parentHp);
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
        Debug.Log("Door Closed");
    }
    
    private void OpenDoors()
    {
        Debug.Log("Door Opened");
    }
}
