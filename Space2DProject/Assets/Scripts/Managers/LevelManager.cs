using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private GenerationSimpleHalf generator;
    
    [SerializeField] private int firstSeed;
    [SerializeField] private int numberOfRooms;

    private List<int> seedList = new List<int>();

    private int floorNumber;
    
    void Start()
    {
        generator = gameObject.GetComponent<GenerationSimpleHalf>();
        
        floorNumber = 0;
        
        seedList.Add(firstSeed);
        
        generator.GenerateRooms(numberOfRooms,generator.GetGrid(),firstSeed);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ClearLevel();
        }

        if (Input.GetKey(KeyCode.B))
        {
            GenerateNewLevel();
        }
    }

    int GetNewSeed()
    {
        Random.InitState(firstSeed);
        
        int seed = Random.Range(0, 999999999);
        
        for (int i = 0; i < floorNumber-1; i++)
        {
            seed = Random.Range(0, 999999999);
            Debug.Log(seed);
        }

        seed += firstSeed + floorNumber;
        
        return seed;
    }

    void GenerateNewLevel()
    {
        seedList.Add(GetNewSeed());
        floorNumber++;

        generator.GenerateRooms(numberOfRooms,generator.GetGrid(),seedList[floorNumber]);
    }

    void ClearLevel()
    {
        gameObject.GetComponent<NavMeshSurface2d>().RemoveData();

        foreach (Transform child in generator.GetGrid().parent)
        {
            foreach (Transform item in child)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
