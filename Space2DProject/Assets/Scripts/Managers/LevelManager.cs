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

    [SerializeField] private GameObject bossRoom;
    [SerializeField] private GameObject bossStartRoom;

    private bool canGenerate = true;
    
    #region Singleton

    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        generator = gameObject.GetComponent<GenerationSimpleHalf>();
        
        seedList.Add(firstSeed);
        
        if (!generateOnStart) return;
        StartNewRun(numberOfRooms,firstSeed);
    }

    public void StartNewRun(int rooms, int seed)
    {
        if (!canGenerate) return;

        if (rooms < 0)
        {
            rooms = -rooms;
            seed = (int) Time.realtimeSinceStartup;
        }
        
        floorNumber = 0;
        
        seedList.Clear();
        
        seedList.Add(seed);
            
        canGenerate = false;
            
        StartCoroutine(ResetRun(rooms,seed));
    }

    public Transform GetLastRoom()
    {
        return generator.GetLastRoom().transform;
    }

    private int GetNewSeed()
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

    private void ClearLevel()
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

    IEnumerator CurrentLevel()
    {
        ClearLevel();

        yield return null;
        
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
            if (!canGenerate) return;
            canGenerate = false;
            StartCoroutine(NewLevel());
        }
        else
        {
            //start Coroutine(StartBossLevel)
            ClearLevel();

            //ClearLevel
            //Instantiate rooms at correct pos (shop too)
            //Instantiate(bossRoom);
            //Instantiate(bossStartRoom);
            //Deactivate Portal
            //Boss Parameters (other script)
            
            
            floorNumber--;
            ConsoleManager.Instance.Print("Max Level Reached");
        }

        
    }

    public void GeneratePreviousLevel()
    {
        if (!canGenerate) return;
        canGenerate = false;
        StartCoroutine(PreviousLevel());
    }

    public void ReloadLevel()
    {
        if (!canGenerate) return;
        canGenerate = false;
        StartCoroutine(CurrentLevel());
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
        var floor = floorNumber + 1;

        if (floor <= maxFloors * 1 / 3)
        {
            return 0;
        }
                
        return floor <= maxFloors * 2 / 3 ? 1 : 2;
    }

    public int GetMaxFloors()
    {
        return maxFloors;
    }

    public int SetMaxFloors(int n)
    {
        maxFloors = n >= 1 ? n : 1;
        return maxFloors;
    }
}
