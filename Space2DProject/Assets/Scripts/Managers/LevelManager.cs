using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    [SerializeField] private RawImage minimapBackground;
    [SerializeField] private RawImage mapBackground;
    [SerializeField] private Camera mapCamera;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Image fogImage;
    [SerializeField] private Sprite portalBiome1;
    [SerializeField] private Sprite portalBiome2;
    [SerializeField] private Sprite portalBiome3;
    [SerializeField] private Sprite towerBiome1;
    [SerializeField] private Sprite towerBiome2;
    [SerializeField] private Sprite towerBiome3;
    
    [SerializeField] private int firstSeed;
    [SerializeField] private int numberOfRooms;

    [SerializeField] private List<int> seedList = new List<int>();

    public int floorNumber;
    [SerializeField] private int maxFloors = 3;

    private AudioManager am;

    private bool canGenerate = true;

    [SerializeField] private Dialogues dialogue;
    
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
        am = AudioManager.Instance;
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
        PierreTombaleLinear.usableDialogues.Clear();
        PierreTombaleLinear.index = 0;
        
        gameObject.GetComponent<NavMeshSurface2d>().RemoveData();

        generator.CleanUpObjects();

        foreach (Transform item in ObjectPooler.Instance.transform)
        {
            item.gameObject.SetActive(false);
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

        UIManager.Instance.IncreaseScore(0);

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
        else if (floorNumber == maxFloors)
        {
            
            ClearLevel();
            StartCoroutine(BossFightNavMesh());
            
            generator.GeneratorSettingsForBoss();
            
            ConsoleManager.Instance.Print("Max Level Reached");
        }
        else
        {
            LoadingManager.Instance.UpdateLoading(0);
            SceneManager.LoadScene(6);
        }
        

    }

    public void ChangeBackgroundColor()
    {
        var biome = GetBiome();
        var cam = mainCamera.GetComponent<Camera>();
        var portalSpriteRenderer = generator.level.GetChild(3).GetComponent<SpriteRenderer>();
        var towerSpriteRenderer = generator.level.GetChild(7).GetComponent<SpriteRenderer>();
        switch (biome)
        {
            case 0:
                mapCamera.backgroundColor = mapBackground.color = minimapBackground.color = cam.backgroundColor = new Color(0.01176471f,0.0627451f,0.1176471f,1f);
                globalLight.color = new Color(0.4237718f,0.7010058f,0.8396226f,1);
                portalSpriteRenderer.sprite = portalBiome1;
                towerSpriteRenderer.sprite = towerBiome1;
                fogImage.color = new Color(0.2509804f,0.3294118f,1f,1f);
                am.Stop(5,true);
                StartCoroutine(LatePlay(6, 3));
                break;
            case 1:
                mapCamera.backgroundColor = mapBackground.color = minimapBackground.color = cam.backgroundColor = new Color(0.3960784f, 0.05882353f, 0.09411765f, 1);
                globalLight.color = new Color(0.8396226f,0.6663744f,0.4475347f, 1);
                portalSpriteRenderer.sprite = portalBiome2;
                towerSpriteRenderer.sprite = towerBiome2;
                fogImage.color = new Color(1f,0f,0.1058824f,1);
                am.Stop(6,true);
                StartCoroutine(LatePlay(7, 3));
                break;
            case 2:
                mapCamera.backgroundColor = mapBackground.color = minimapBackground.color = cam.backgroundColor = new Color(0.09411765f, 0.07843138f, 0.07843138f, 1);
                globalLight.color = new Color(0.2641509f,0.2159546f,0.1831613f,1f);
                portalSpriteRenderer.sprite = portalBiome3;
                towerSpriteRenderer.sprite = towerBiome3;
                fogImage.color = new Color(0.7098039f,0.7098039f,0.7098039f,1f);
                am.Stop(7,true);
                StartCoroutine(LatePlay(8, 3));
                break;
            default:
                mapCamera.backgroundColor = mapBackground.color = minimapBackground.color = cam.backgroundColor = new Color(0.03137255f, 0.09019608f, 0.145098f, 1);
                globalLight.color = Color.white;
                portalSpriteRenderer.sprite = portalBiome1;
                towerSpriteRenderer.sprite = towerBiome1;
                fogImage.color = new Color(0.2509804f,0.3294118f,1f,1f);
                break;
        }
    }

    public IEnumerator LatePlay(int id, float time)
    {
        yield return new WaitForSeconds(time);
        am.Stop(id);
        
        am.Play(id);
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

    public Camera Camera()
    {
        return mainCamera.gameObject.GetComponent<Camera>();
    }

    public void GoToBossFight()
    {
        AudioManager.Instance.StopAllSounds();
        floorNumber = maxFloors - 1;
        GenerateNextLevel();
        ChangeBackgroundColor();
        Level().GetChild(6).gameObject.SetActive(true);
        generator.GeneratorSettingsForBoss();
    }

    public void PlayIntroDialogue()
    {
        if(LoadingLevelData.Instance.firstRunDialogue) return;
        DialogueManager.Instance.StartDialogue(dialogue);
        LoadingLevelData.Instance.firstRunDialogue = true;

    }
}
