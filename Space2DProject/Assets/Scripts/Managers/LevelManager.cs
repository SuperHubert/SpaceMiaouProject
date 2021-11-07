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
    [SerializeField] private Transform mainCamera;
    
    [SerializeField] private int firstSeed;
    [SerializeField] private int numberOfRooms;

    [SerializeField] private List<int> seedList = new List<int>();

    [SerializeField] private int floorNumber;

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
        
        StartNewRun(numberOfRooms,firstSeed);
    }

    public void StartNewRun(int rooms, int seed)
    {
        if (canGenerate)
        {
            floorNumber = 0;
        
            seedList.Clear();
        
            seedList.Add(seed);
            
            canGenerate = false;
            
            StartCoroutine(ResetRun(rooms,seed));
        }
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

    public void MovePlayer(Transform position)
    {
        mainCamera.position = player.position = position.position;
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

    IEnumerator ResetRun(int rooms, int seed)
    {
        ClearLevel();

        yield return null;
        
        generator.GenerateRooms(rooms,seed);

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

    public GameObject Player()
    {
        return player.gameObject;
    }

    public int FloorNumber()
    {
        return floorNumber;
    }
}
