using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private GenerationSimpleHalf generator;

    [SerializeField] private Transform player;
    
    [SerializeField] private int firstSeed;
    [SerializeField] private int numberOfRooms;

    [SerializeField] private List<int> seedList = new List<int>();

    private int floorNumber;

    private bool canGenerate = true;
    
    #region Singleton

    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
    
    void Start()
    {
        generator = gameObject.GetComponent<GenerationSimpleHalf>();
        
        floorNumber = 0;
        
        seedList.Add(firstSeed);
        
        GenerateNewLevel();
    }
    
    int GetNewSeed()
    {
        Random.InitState(firstSeed);

        int seed = firstSeed;
        
        for (int i = 0; i < floorNumber+1; i++)
        {
            seed = Random.Range(0, 999999999);
        }

        seed += firstSeed + floorNumber;
        
        return seed;
    }

    void GenerateNewLevel()
    {
        seedList.Add(GetNewSeed());
        floorNumber++;
        
        generator.GenerateRooms(numberOfRooms,seedList[floorNumber]);
    }

    public void MovePlayer()
    {
        player.position = generator.spawnPoint;
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

    IEnumerator NewLevel()
    {
        ClearLevel();

        yield return null;
        
        GenerateNewLevel();

        canGenerate = true;
    }

    public void Generate()
    {
        if (canGenerate)
        {
            canGenerate = false;
            StartCoroutine(NewLevel());
        }
        
    }
}
