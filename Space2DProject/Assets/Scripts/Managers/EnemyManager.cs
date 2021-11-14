using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyList;

    public void GetList()
    {
        for (int i = 0; i < enemyList.Capacity; i++)
        {
            ConsoleManager.Instance.Print("id : "+i+" - "+enemyList[i].name);
        }
        
    }

    public void SpawnEnemy(int id, float x, float y)
    {
        if (id >= enemyList.Count)
        {
            ConsoleManager.Instance.Print("Invalid id, use 'enemylist' for a list of all enemies");
            return;
        }

        var spawnPoint = new Vector3(x, y, 0);

        if (!IsOnNavMesh(spawnPoint))
        {
            ConsoleManager.Instance.Print("Invalid Coordinates, must be in the region");
            return;
        }
        
        Instantiate(enemyList[id],spawnPoint, Quaternion.identity,LevelManager.Instance.Level().GetChild(1));

        ConsoleManager.Instance.Print("Spawned " + enemyList[id].name + " at " + x + " " + y);
        ConsoleManager.Instance.Print("Kind of Broken");
    }
    
    private bool IsOnNavMesh(Vector3 targetDestination)
    {
        return NavMesh.SamplePosition(targetDestination, out _, 1f, NavMesh.AllAreas);
    }
}
