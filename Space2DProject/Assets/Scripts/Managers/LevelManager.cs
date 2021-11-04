using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private GenerationSimpleHalf script;
    
    [SerializeField] private int firstSeed;
    [SerializeField] private int numberOfRooms;

    private List<int> seedList = new List<int>();

    private int floorNumber;
    
    void Start()
    {
        script = gameObject.GetComponent<GenerationSimpleHalf>();
        
        floorNumber = 0;
        
        seedList.Add(firstSeed);
        
        script.GenerateRooms(numberOfRooms,script.GetGrid(),firstSeed);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            StartCoroutine(NewLevel());
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

    void GenerateNewLevel()
    {

        seedList.Add(GetNewSeed());
        floorNumber++;

        script.GenerateRooms(numberOfRooms,script.GetGrid(),seedList[floorNumber]);
    }

    void ClearLevel()
    {
        gameObject.GetComponent<NavMeshSurface2d>().RemoveData();

        foreach (Transform child in script.GetGrid().parent)
        {
            foreach (Transform item in child)
            {
                Destroy(item.gameObject);
            }
        }
    }

    IEnumerator NewLevel()
    {
        ClearLevel();

        yield return new WaitForSeconds(1);
        
        GenerateNewLevel();
    }
    
}
