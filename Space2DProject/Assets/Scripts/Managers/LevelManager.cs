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

    [SerializeField] private GameObject bossFightObj;
    private BossFight bossfight;

    [SerializeField] private bool generateOnStart = true;

    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerFall;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private FollowPlayer playerFollower;
    
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

    private void Start()
    {
        generator = gameObject.GetComponent<GenerationSimpleHalf>();
        bossfight = bossFightObj.GetComponent<BossFight>();

        seedList.Add(firstSeed);
        
        if (!generateOnStart) return;
        StartNewRun(numberOfRooms,firstSeed);
    }

    public void StartNewRun(int rooms, int seed)
    {
        if (!canGenerate) return;
        playerFollower.isInHub = true;

        if (rooms < 0)
        {
            if (LoadingLevelData.Instance != null)
            {
                rooms = LoadingLevelData.Instance.numberOfRooms;
                seed = LoadingLevelData.Instance.seed;
                maxFloors = LoadingLevelData.Instance.maxFloors;
            }
            
        }
        
        floorNumber = 0;
        
        seedList.Clear();
        
        seedList.Add(seed);
            
        canGenerate = false;
            
        StartCoroutine(ResetRun(rooms,seed));
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
    
    private IEnumerator NewLevel()
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

    private IEnumerator PreviousLevel()
    {
        ClearLevel();

        yield return null;

        if (floorNumber == maxFloors)
        {
            bossfight.CancelBossFight();
        }
        
        floorNumber--;
        if (floorNumber < 0)
        {
            floorNumber = 0;
        }
        
        generator.GenerateRooms(numberOfRooms,seedList[floorNumber]);

        canGenerate = true;
    }

    private IEnumerator CurrentLevel()
    {
        ClearLevel();

        yield return null;
        
        generator.GenerateRooms(numberOfRooms,seedList[floorNumber]);

        canGenerate = true;
    }
    
    private IEnumerator ResetRun(int rooms, int seed)
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
            
            ClearLevel();
            StartCoroutine(BossFightNavMesh());
            
            generator.GeneratorSettingsForBoss();
            
            //Boss Parameters (other script)
            
            //floorNumber--;
            ConsoleManager.Instance.Print("Max Level Reached");
        }
        
    }

    IEnumerator BossFightNavMesh()
    {
        bossfight.ActivateBossFight();
        for (var i = 0; i < 60; i++)
        {
            if (i == 12)
            {
                yield return null;
                gameObject.GetComponent<NavMeshSurface2d>().BuildNavMesh();
            }
            LoadingManager.Instance.UpdateLoading(i/60f);
            yield return null;
        }
        
        MovePlayer(bossfight.SpawnBossAndReturnStartPos());
        
        LoadingManager.Instance.UpdateLoading(2);
        
    }

    public void MovePlayer(Transform position)
    {
        mainCamera.position = player.position = position.position;
        if (playerFall != null && playerFall.GetComponent<Fall>() != null)
        {
            StartCoroutine(playerFall.GetComponent<Fall>().TeleportFollower(true));
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
    
    public Transform GetLastRoom()
    {
        return generator.GetLastRoom().transform;
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
