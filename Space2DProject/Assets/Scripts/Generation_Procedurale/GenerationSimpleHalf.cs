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
    private Transform spawnPoint;
    private Transform shopPos;
    private Transform portalPos;
    private Transform healthBarCanvas;
    private Transform lights;
    private Transform tower;
    private int roomSpriteRendersIndex;
    private int roomTilemapsIndex;
    private int roomCollisionsIndex;
    private int roomEnemiesIndex;
    private int roomOtherIndex;
    private int roomPortalIndex;
    private int roomSpawnIndex;
    private int roomShopIndex;
    private int roomTowerIndex;
    [SerializeField] private Camera cameraMap;
    [SerializeField] private FollowPlayer followPlayer;
    [SerializeField] private FogOfWar fogOfWar;

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

    private TextureAssigner textureAssigner;
    private void Awake()
    {
        grid = level.GetChild(0);
        enemies = level.GetChild(1);
        items = level.GetChild(2);
        spawnPoint = level.GetChild(4);
        portalPos = level.GetChild(3);
        healthBarCanvas = level.GetChild(5);
        shopPos = level.GetChild(6);
        tower = level.GetChild(7);

        roomSpriteRendersIndex = 0;
        roomTilemapsIndex = 1;
        roomCollisionsIndex = 2;
        roomEnemiesIndex = 3;
        roomOtherIndex = 4;
        roomPortalIndex = 5;
        roomSpawnIndex = 6;
        roomShopIndex = 7;
        roomTowerIndex = 8;
    }

    private void Start()
    {
        textureAssigner = gameObject.GetComponent<TextureAssigner>();
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
        
        textureAssigner.FillAllPools();

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
        
        SetSpawnAndShopPoints();
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
                currentRoom = checkNextInsteadOfPrevious ? GetCaseFromNumber(currentRoom.generationNumber + 1) : GetCaseFromNumber(currentRoom.generationNumber - 1);
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
        textureAssigner.FillAllPools();
        
        LevelManager.Instance.ChangeBackgroundColor();
        
        foreach (Case room in generationList)
        {
            GetSurroundingCasesList(room);
            room.CloseOutOfBoundsWalls();
            
            GameObject prefabRoom = textureAssigner.GetRoom((room.caseAbove != null), (room.caseUnder != null),
                (room.caseLeft != null), (room.caseRight != null));

            room.GetComponent<SpriteRenderer>().sprite = prefabRoom.GetComponent<SpriteRenderer>().sprite;
            
            //Instantiates prefab tilemap UnWalkable
            Instantiate(prefabRoom.transform.GetChild(roomTilemapsIndex).GetChild(1),room.transform.GetChild(roomTilemapsIndex));
            
            //Instantiates prefab collision GameObjects
            foreach (Transform collisionObj in prefabRoom.transform.GetChild(roomCollisionsIndex))
            {
                Instantiate(collisionObj, room.transform.GetChild(roomCollisionsIndex));
            }
            
            //Instantiates prefab Sprite Renderers GameObjects
            foreach (Transform spriteRenObj in prefabRoom.transform.GetChild(roomSpriteRendersIndex))
            {
                Instantiate(spriteRenObj, room.transform.GetChild(roomSpriteRendersIndex));
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
            LevelManager.Instance.MovePlayer(spawnPoint);
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
        textureAssigner.FillAllPools();
        
        foreach (Case room in generationList)
        {

            GameObject prefabRoom = textureAssigner.GetRoom((room.caseAbove != null),
                (room.caseUnder != null),
                (room.caseLeft != null), (room.caseRight != null));
            
            //Instantiates prefab enemy GameObjects (not for 1st room)
            if (room != generationList[0])
            {
                foreach (Transform enemyObj in prefabRoom.transform.GetChild(roomEnemiesIndex))
                {
                    Transform instantiatedEnemy = Instantiate(enemyObj, room.transform);
                    instantiatedEnemy.parent = enemies;
                    //instantiatedEnemy.GetChild(0).GetChild(1).SetParent(healthBarCanvas);
                    Debug.Log(instantiatedEnemy.GetChild(0));
                    instantiatedEnemy.GetChild(0).GetComponent<EnemyHealth>().healthBarTransform.SetParent(healthBarCanvas);
                }
            }
            
            //Instantiates prefab items GameObjects
            foreach (Transform otherObj in prefabRoom.transform.GetChild(roomOtherIndex))
            {
                Transform chest = Instantiate(otherObj, room.transform);
                chest.parent = items;
                if (otherObj.GetComponent<Chest>() != null)
                {
                    AddChest(chest.gameObject);
                }
            }
            
            UpdateProgress(0.1f/ dungeonNumberOfRooms);
            yield return null;
        }
        
        Random.state = randState;

        followPlayer.isInHub = false;
        
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

        level.transform.position = new Vector3((-targetPos.x*50)-25,(-targetPos.y*50)-25,0);
    }
    
    private void CleanUpGrid()
    {
        textureAssigner.FillAllPools();
        
        for (int i = 0; i < (2*dungeonNumberOfRooms+1); i++)
        {
            for (int j = 0; i < (2*dungeonNumberOfRooms+1); i++)
            {
                generationGrid[i, j] = null;
            }
        }
        
        generationList.Clear();
        
        fogOfWar.ClearFog();
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
    
    private void SetSpawnAndShopPoints()
    {
        Case room = generationList[0];
        
        GameObject posObject = Instantiate(firstRoomPrefab.transform.GetChild(roomSpawnIndex).gameObject, room.transform);

        spawnPoint.position = posObject.transform.position;
        
        posObject = Instantiate(firstRoomPrefab.transform.GetChild(roomShopIndex).gameObject, room.transform);
        
        shopPos.position = posObject.transform.position;
        
        shopPos.gameObject.GetComponent<ShopInteraction>().UpdateAppearance();
    }

    private void MovePortal()
    {
        Case room = generationList[dungeonNumberOfRooms-1];
        
        GameObject posObject = Instantiate(lastRoomPrefab.transform.GetChild(roomPortalIndex).gameObject, room.transform);

        portalPos.position = posObject.transform.position;
        
        //posObject = Instantiate(lastRoomPrefab.transform.GetChild(roomTowerIndex).gameObject, room.transform);
        posObject = Instantiate(lastRoomPrefab.transform.GetChild(roomSpawnIndex).gameObject, room.transform);
        
        tower.position = posObject.transform.position;
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
            fogOfWar.levelSize = levelHeight * 25 + 10;
        }
        else
        {
            cameraMap.orthographicSize = levelWidth * 25 + 10;
            fogOfWar.levelSize = levelWidth * 25 + 10;
        }

    }

    private void DestroySomeChests()
    {
        foreach (GameObject item in chestList)
        {
            item.GetComponent<Chest>().UpdateChest();
        }
    }
    
    public void GeneratorSettingsForBoss()
    {
        portalPos.position = new Vector3(0, -150, 0);
        
        cameraMap.orthographicSize = 25 + 10;
        
        //maybe more stuff
    }

    public void CleanUpObjects()
    {
        foreach (Transform child in healthBarCanvas)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in enemies)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in items)
        {
            Destroy(child.gameObject);
        }

        tower.GetComponent<Collider2D>().enabled = true;
        tower.GetChild(0).gameObject.SetActive(false);
        portalPos.GetComponent<Collider2D>().enabled = false;
        
        if (lights == null) return;
        foreach (Transform child in lights)
        { 
            Destroy(child.gameObject);
        }

    }
}