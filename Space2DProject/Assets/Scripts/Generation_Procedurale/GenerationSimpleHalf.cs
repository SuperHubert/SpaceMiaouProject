using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GenerationSimpleHalf : MonoBehaviour
{
    private int dungeonSeed;
    private int dungeonNumberOfRooms;
    
    public Transform level;
    private Transform grid;
    private Transform enemies;
    private Transform items;
    [SerializeField] private Camera cameraMap;

    [SerializeField] private bool buildNavMesh = true;
    [SerializeField] private bool movePlayer = true;
    
    [SerializeField] private GameObject firstRoomPrefab;
    [SerializeField] private GameObject lastRoomPrefab;
    private float progress = 0;
    private List<GameObject> chestList = new List<GameObject>();

    Case[,] generationGrid;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private List<Case> generationList;

    private int checkpointNumber;
    private bool checkNextInsteadOfPrevious = false;

    private Random.State randState;
    private void Awake()
    {
        grid = level.GetChild(0);
        enemies = level.GetChild(1);
        items = level.GetChild(2);
    }

    public List<int> roomDispersion;
    [SerializeField] private List<float> allRooms = new List<float> {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

    private void Start()
    {
        
    }

    public int numberOfInterations;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 0; i < numberOfInterations; i++)
            {
                RoomRepartition();
            }
            
        }
    }

    private void RoomRepartition()
    {
        foreach (Transform child in GetGrid().parent)
        { 
            foreach (Transform item in child)
            {
                Destroy(item.gameObject);
            }
        }
            
        Random.InitState(Random.Range(0, 999999999));
            
        InitVariables(dungeonNumberOfRooms);
            
        //PlaceRooms
        Case selectedCase = CreateRoom(roomPrefab.GetComponent<Case>(),new Vector2Int(dungeonNumberOfRooms,dungeonNumberOfRooms), 0,"0",grid);

        for (int i = 1; i < dungeonNumberOfRooms; i++)
        {
            checkpointNumber = selectedCase.generationNumber;
            checkNextInsteadOfPrevious = false;
            Vector2Int nextPos = GetNextPosition(selectedCase);

            if (Random.Range(0, 2) == 1)
            {
                Case newRoom = CreateRoom(roomPrefab.GetComponent<Case>(),nextPos, i, i.ToString(),grid);
                UpdateAllSurroundingCases(newRoom);
            }
            else
            {
                selectedCase = CreateRoom(roomPrefab.GetComponent<Case>(),nextPos, i, i.ToString(),grid);
                UpdateAllSurroundingCases(selectedCase);
            }
                
        }

        randState = Random.state;

        //UpdateRoomAppearance
        foreach (Case room in generationList)
        {
            GetSurroundingCasesList(room);
            room.CloseOutOfBoundsWalls();
            
            GameObject prefabRoom = gameObject.GetComponent<TextureAssigner>().GetRoom((room.caseAbove != null), (room.caseUnder != null),
                    (room.caseLeft != null), (room.caseRight != null));

            room.GetComponent<SpriteRenderer>().sprite = prefabRoom.GetComponent<SpriteRenderer>().sprite;
            
            //Instantiates prefab tilemap UnWalkable
            Instantiate(prefabRoom.transform.GetChild(0).GetChild(1),room.transform.GetChild(0));
            
            //Instantiates prefab collision GameObjects
            foreach (Transform item in prefabRoom.transform.GetChild(1))
            {
                    Instantiate(item, room.transform.GetChild(1));
            }

            if (firstRoomPrefab == null || firstRoomPrefab.activeSelf==false)
            {
                firstRoomPrefab = prefabRoom;
            }
            lastRoomPrefab = prefabRoom;
                
        }
            
        CleanUpGrid();
            
        //Convert to percentage
        int total = 0;
        foreach (int number in roomDispersion)
        {
            total += number;
        }

        float numberorooms = 0;
        for (int i = 0; i < roomDispersion.Count; i++)
        {
            allRooms[i] = 16*(float)roomDispersion[i] / total;
            numberorooms += allRooms[i];
        }

        allRooms[15] = numberorooms;
        allRooms[16]++;
    }
    public void GenerateRooms(int numberOfRooms,int seed)
    {
        Random.InitState(seed);
        
        InitVariables(numberOfRooms);

        StartCoroutine(PlaceRooms());
        
        /*
        PlaceRooms();
        
        UpdateRoomAppearance();
        
        RecenterLevel();
        
        BuildNavMesh();
        
        SpawnEnemiesAndItems();
        
        Random.state = generationOver;
        
        SetSpawnPoint();
        
        MovePortal();
        
        CleanUpGrid();
        */
        
    }

    public Transform GetGrid()
    {
        return grid;
    }

    public GameObject GetLastRoom()
    {
        return grid.GetChild(dungeonNumberOfRooms-1).gameObject;
    }

    public bool ToggleNavMesH()
    {
        return buildNavMesh = !buildNavMesh;
    }

    public bool ToggleTeleport()
    {
        return movePlayer = !movePlayer;
    }
    
    private void AddChest(GameObject chest)
    {
        chestList.Add(chest);
    }
    
    private void InitVariables(int number)
    {
        firstRoomPrefab = null;
        
        level.position = Vector3.zero;

        dungeonNumberOfRooms = number;
        
        generationGrid = new Case[2*(number)+1,2*(number)+1];

        progress = 0;

        chestList.Clear();

        UpdateProgress(0.01f);
    }
    
    IEnumerator PlaceRooms()
    {
        Case selectedCase = CreateRoom(roomPrefab.GetComponent<Case>(),new Vector2Int(dungeonNumberOfRooms,dungeonNumberOfRooms), 0,"0",grid);

        for (int i = 1; i < dungeonNumberOfRooms; i++)
        {
            checkpointNumber = selectedCase.generationNumber;
            checkNextInsteadOfPrevious = false;
            Vector2Int nextPos = GetNextPosition(selectedCase);

            if (Random.Range(0, 2) == 1)
            {
                Case newRoom = CreateRoom(roomPrefab.GetComponent<Case>(),nextPos, i, i.ToString(),grid);
                UpdateAllSurroundingCases(newRoom);
            }
            else
            {
                selectedCase = CreateRoom(roomPrefab.GetComponent<Case>(),nextPos, i, i.ToString(),grid);
                UpdateAllSurroundingCases(selectedCase);
            }

            UpdateProgress(0.25f / dungeonNumberOfRooms);
            yield return null;
        }

        randState = Random.state;

        StartCoroutine(UpdateRoomAppearance());
        
        SetSpawnPoint();
    }
    
    private Case CreateRoom(Case creator,Vector2Int coords,int creationNumber, string caseName ,Transform caseParent)
    {
        Case createdRoom = creator.CreateCase(coords,dungeonNumberOfRooms,creationNumber);
        createdRoom.position = coords;
        createdRoom.name = caseName;
        createdRoom.transform.parent = caseParent;
        
        generationGrid[coords.x, coords.y] = createdRoom;
        generationList.Add(generationGrid[coords.x, coords.y]);
        
        return generationGrid[coords.x, coords.y];
    }

    private Vector2Int GetNextPosition(Case currentRoom)
    {
        List<int> surroundingRoomsList = GetSurroundingCasesList(currentRoom);

        if (surroundingRoomsList.Count == 4)
        {
            if (currentRoom.generationNumber - 1 < 0 && !checkNextInsteadOfPrevious)
            {
                currentRoom = GetCaseFromNumber(checkpointNumber);
                checkNextInsteadOfPrevious = true;
            }
            else
            {
                if (checkNextInsteadOfPrevious)
                {
                    currentRoom = GetCaseFromNumber(currentRoom.generationNumber + 1);
                }
                else
                {
                    currentRoom = GetCaseFromNumber(currentRoom.generationNumber - 1);
                }
            }
            
            return GetNextPosition(currentRoom);
        }
        else
        {
            List<int> availablePositionsList = GetAvailablePositionsListFromUnavailablePositionsList(surroundingRoomsList);

            int selectedPosition = availablePositionsList[Random.Range(0, availablePositionsList.Count)];

            return ConvertIntToPosition(selectedPosition, currentRoom);
        }
    }
    
    private List<int> GetSurroundingCasesList(Case targetCase)
    {
        UpdateAllSurroundingCases(targetCase);
        List<int> caseList = new List<int>();
        
        if (targetCase.caseAbove != null)
        {
            caseList.Add(1);
        }

        if (targetCase.caseUnder != null)
        {
            caseList.Add(3);
        }
        
        if (targetCase.caseRight != null)
        {
            caseList.Add(2);
        }

        if (targetCase.caseLeft != null)
        {
            caseList.Add(4);
        }
        
        return caseList;
    }
    
    private void UpdateAllSurroundingCases(Case targetCase)
    {
        if (generationGrid[targetCase.position.x, targetCase.position.y + 1] != null)
        {
            targetCase.caseAbove = generationGrid[targetCase.position.x, targetCase.position.y + 1];
        }
        if (generationGrid[targetCase.position.x, targetCase.position.y - 1] != null)
        {
            targetCase.caseUnder = generationGrid[targetCase.position.x, targetCase.position.y - 1];
        }
        if (generationGrid[targetCase.position.x + 1, targetCase.position.y] != null)
        {
            targetCase.caseRight = generationGrid[targetCase.position.x + 1, targetCase.position.y];
        }
        if (generationGrid[targetCase.position.x - 1, targetCase.position.y] != null)
        {
            targetCase.caseLeft = generationGrid[targetCase.position.x - 1, targetCase.position.y];
        }
    }
    
    private Case GetCaseFromNumber(int number)
    {
        foreach (Case room in generationList)
        {
            if (room.generationNumber == number)
            {
                return room;
            }
        }
        
        return null;
    }
    
    private List<int> GetAvailablePositionsListFromUnavailablePositionsList(List<int> unavailablePositionsList)
    {
        List<int> availablePositionsList = new List<int>() {1,2,3,4};
        foreach (int position in unavailablePositionsList)
        {
            if (availablePositionsList.Contains(position))
            {
                availablePositionsList.Remove(position);
            }
        }

        return availablePositionsList;
    }
    
    private Vector2Int ConvertIntToPosition(int position,Case room)
    {
        switch (position)
        {
            case 1:
                return new Vector2Int(room.position.x, room.position.y + 1);
            case 2:
                return new Vector2Int(room.position.x + 1, room.position.y);
            case 3:
                return new Vector2Int(room.position.x, room.position.y - 1);
            case 4:
                return new Vector2Int(room.position.x - 1, room.position.y);
            default:
                return Vector2Int.zero;
        }
    }
    
    IEnumerator UpdateRoomAppearance()
    {
        foreach (Case room in generationList)
        {
            GetSurroundingCasesList(room);
            room.CloseOutOfBoundsWalls();
            
            GameObject prefabRoom = gameObject.GetComponent<TextureAssigner>().GetRoom((room.caseAbove != null), (room.caseUnder != null),
                (room.caseLeft != null), (room.caseRight != null));

            room.GetComponent<SpriteRenderer>().sprite = prefabRoom.GetComponent<SpriteRenderer>().sprite;
            
            //Instantiates prefab tilemap UnWalkable
            Instantiate(prefabRoom.transform.GetChild(0).GetChild(1),room.transform.GetChild(0));
            
            //Instantiates prefab collision GameObjects
            foreach (Transform item in prefabRoom.transform.GetChild(1))
            {
                Instantiate(item, room.transform.GetChild(1));
            }

            if (firstRoomPrefab == null || firstRoomPrefab.activeSelf==false)
            {
                firstRoomPrefab = prefabRoom;
            }
            lastRoomPrefab = prefabRoom;

            UpdateProgress(0.25f / dungeonNumberOfRooms);
            yield return null;
        }
        
        MovePortal();
        
        RecenterLevel();


        if (movePlayer)
        {
            LevelManager.Instance.MovePlayer(level.GetChild(4));
        }
        
        SetCameraSize();

        StartCoroutine(BuildNavMesh());
    }
    
    IEnumerator BuildNavMesh()
    {
        if (buildNavMesh)
        {
            gameObject.GetComponent<NavMeshSurface2d>().BuildNavMesh();
        }
        
        UpdateProgress(0.39f);
        yield return null;

        Random.state = randState;
        
        StartCoroutine(SpawnEnemiesAndItems());
    }
    
    IEnumerator SpawnEnemiesAndItems()
    {
        foreach (Case room in generationList)
        {
            GameObject prefabRoom = gameObject.GetComponent<TextureAssigner>().GetRoom((room.caseAbove != null),
                (room.caseUnder != null),
                (room.caseLeft != null), (room.caseRight != null));
            
            //Instantiates prefab enemy GameObjects (not for 1st room)
            if (room != generationList[0])
            {
                foreach (Transform item in prefabRoom.transform.GetChild(2))
                {
                    Instantiate(item, room.transform).parent = enemies;
                }
            }
            
            //Instantiates prefab items GameObjects
            foreach (Transform item in prefabRoom.transform.GetChild(3))
            {
                Transform chest = Instantiate(item, room.transform);
                chest.parent = items;
                if (item.GetComponent<Chest>() != null)
                {
                    AddChest(chest.gameObject);
                }
            }
            
            UpdateProgress(0.1f/ dungeonNumberOfRooms);
            yield return null;
        }
        
        Random.state = randState;
        
        DestroySomeChests();
        
        CleanUpGrid();

        UpdateProgress(1);
    }

    private void RecenterLevel()
    {
        int[] levelBounds = GetLevelSize();
        
        int levelMinY = levelBounds[1];
        int levelMinX = levelBounds[3];
       
        int levelHeight = levelBounds[0] - levelMinY + 1;
        int levelWidth = levelBounds[2] - levelMinX + 1;
        int currentCenterPosX = generationList[0].position.x - levelMinX + 1;
        int currentCenterPosY = generationList[0].position.y - levelMinY + 1;
        
        Vector2 targetPos = new Vector2((levelWidth / 2f)-currentCenterPosX, (levelHeight / 2f)-currentCenterPosY);
        Debug.Log(targetPos);

        level.transform.position = new Vector3((-targetPos.x*50)-25,(-targetPos.y*50)-25,0);
    }
    
    private void CleanUpGrid()
    {
        for (int i = 0; i < (2*dungeonNumberOfRooms+1); i++)
        {
            for (int j = 0; i < (2*dungeonNumberOfRooms+1); i++)
            {
                generationGrid[i, j] = null;
            }
        }
        
        generationList.Clear();
    }
    
    private int[] GetLevelSize()
    {
        int[] values = new int[4]; 
        Case aya = generationList[0]; 
        values[0] = aya.position.y; //top
        values[1] = aya.position.y; //bot
        values[2] = aya.position.x; //right
        values[3] = aya.position.x; //left

        foreach (Case item in generationList)
        {
            if (item.position.y > values[0])
            {
                values[0] = item.position.y;
            }
            if (item.position.y < values[1])
            {
                values[1] = item.position.y;
            }
            
            if (item.position.x > values[2])
            {
                values[2] = item.position.x;
            }
            if (item.position.x < values[3])
            {
                values[3] = item.position.x;
            }
            
        }
        
        return values;
    }
    
    private void SetSpawnPoint()
    {
        Case room = generationList[0];
        
        GameObject posObject = Instantiate(firstRoomPrefab.transform.GetChild(5).gameObject, room.transform);

        level.GetChild(4).position = posObject.transform.position;
    }

    private void MovePortal()
    {
        Case room = generationList[dungeonNumberOfRooms-1];
        
        GameObject posObject = Instantiate(lastRoomPrefab.transform.GetChild(4).gameObject, room.transform);

        level.GetChild(3).position = posObject.transform.position;
    }

    private void UpdateProgress(float number)
    {
        progress += number;
        
        LoadingManager.Instance.UpdateLoading(progress);
    }

    private void SetCameraSize()
    {
        int[] levelBounds = GetLevelSize();
        
        int levelMinY = levelBounds[1];
        int levelMinX = levelBounds[3];
        
        int levelHeight = levelBounds[0] - levelMinY + 1;
        int levelWidth = levelBounds[2] - levelMinX + 1;

        if (levelHeight > levelWidth)
        {
            cameraMap.orthographicSize = levelHeight * 25 + 10;
        }
        else
        {
            cameraMap.orthographicSize = levelWidth * 25 + 10;
        }
        
        
    }

    private void DestroySomeChests()
    {
        foreach (GameObject item in chestList)
        {
            item.GetComponent<Chest>().UpdateChest();
        }
    }
}