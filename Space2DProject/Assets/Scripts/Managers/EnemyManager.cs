using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject SpawnEnemy(int id, float x, float y)
    {
        Instantiate(enemyList[id],new Vector3(x, y, 0),Quaternion.identity);
        
        return enemyList[id];
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
