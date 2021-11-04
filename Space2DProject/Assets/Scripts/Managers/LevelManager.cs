using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject levelGenerator;
    private GenerationSimpleHalf script;
    
    [SerializeField] private int firstSeed;
    [SerializeField] private int numberOfRooms;

    private List<int> seedList = new List<int>();

    private int floorNumber;
    
    void Start()
    {
        script = levelGenerator.GetComponent<GenerationSimpleHalf>();
        
        floorNumber = 0;
        
        seedList.Add(firstSeed);
        
        script.GenerateRooms(numberOfRooms,script.GetGrid(),firstSeed);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            NewLevel(); 
        }
        
    }

    int GetNewSeed()
    {
        Random.InitState(firstSeed);
        
        Debug.Log("Init Seed : "+firstSeed);
        
        int seed = Random.Range(0, 999999999);
        
        Debug.Log("First random : "+seed);
        
        for (int i = 0; i < floorNumber-1; i++)
        {
            seed = Random.Range(0, 999999999);
            Debug.Log(seed);
        }
        Debug.Log("Chosen Random : "+seed);

        seed += firstSeed + floorNumber;
        
        Debug.Log("Final seed :"+seed);

        return seed;
    }

    void NewLevel()
    {
        seedList.Add(GetNewSeed());
        floorNumber++;
        
        ClearLevel();
        
        script.GenerateRooms(numberOfRooms,script.GetGrid(),seedList[floorNumber]);
    }

    void ClearLevel()
    {
        //CLEAR NAVMESH
        
        foreach (Transform child in script.GetGrid().parent)
        {
            foreach (Transform item in child)
            {
                Destroy(item.gameObject);
            }
        }
    }
    
    
}
