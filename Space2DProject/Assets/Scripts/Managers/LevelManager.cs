using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private GenerationSimpleHalf generator;

    [SerializeField] private bool generateOnStart = true;

    [SerializeField] private Transform player;
    [SerializeField] private Transform mainCamera;
    
    [SerializeField] private int firstSeed;
    [SerializeField] private int numberOfRooms;

    [SerializeField] private List<int> seedList = new List<int>();

    [SerializeField] private int floorNumber;
    [SerializeField] private int maxFloors = 3;

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

        if (!generateOnStart) return;
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

    public Transform GetLastRoom()
    {
        return generator.GetLastRoom().transform;
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
        
        if (floorNumber == seedList.Count)
        {
            seedList.Add(GetNewSeed());
        }
        
        generator.GenerateRooms(numberOfRooms,seedList[floorNumber]);

        canGenerate = true;
    }

    IEnumerator PreviousLevel()
    {
        ClearLevel();

        yield return null;
        
        floorNumber--;
        if (floorNumber < 0)
        {
            floorNumber = 0;
        }
        
        generator.GenerateRooms(numberOfRooms,seedList[floorNumber]);

        canGenerate = true;
    }
    
    IEnumerator ResetRun(int rooms, int seed)
    {
        ClearLevel();

        SetSeedAndRoom(rooms,seed);

        yield return null;
        
        generator.GenerateRooms(rooms,seed);

        canGenerate = true;
    }

    public void GenerateNextLevel()
    {
        floorNumber++;

        if (floorNumber < maxFloors)
        {
            if (canGenerate)
            {
                canGenerate = false;
                StartCoroutine(NewLevel());
            }
        }
        else
        {
            floorNumber--;
            ConsoleManager.Instance.Print("Max Level Reached");
        }

        
    }

    public void GeneratePreviousLevel()
    {
        if (canGenerate)
        {
            canGenerate = false;
            StartCoroutine(PreviousLevel());
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

    public void GoToHub()
    {
        SceneManager.LoadScene(3);
    }

    public bool ToggleNavMesh()
    {
        return generator.ToggleNavMesH();
    }

    public bool ToggleTeleport()
    {
        return generator.ToggleTeleport();
    }

    public int GetCurrentSeed()
    {
        return seedList[floorNumber];
    }

    public int GetFirstSeed()
    {
        return firstSeed;
    }

    public int GetCurrentNumberOfRooms()
    {
        return numberOfRooms;
    }
    
    public int GetCurrentFloorNumber()
    {
        return floorNumber;
    }

    public void SetSeedAndRoom(int rooms, int seed)
    {
        numberOfRooms = rooms;

        firstSeed = seed;
    }

    public void AddSeed(int seed)
    {
        seedList.Add(seed);
    }

    public Vector2 GetPos()
    {
        Vector3 pos = player.transform.position;
        return new Vector2(pos.x, pos.y);
    }
    
    public Transform Level()
    {
        return generator.level;
    }

    public int GetBiome()
    {
        if (floorNumber > (2f / 3f) * maxFloors)
        {
            return 2;
        }

        return floorNumber > (1f/3f) * maxFloors ? 1 : 0;
    }

    public int GetMaxFloors()
    {
        return maxFloors;
    }

    public int SetMaxFloors(int n)
    {
        if (n >= 1)
        {
            maxFloors = n;
        }
        else
        {
            maxFloors = 1;
        }
        return maxFloors;
    }
}
