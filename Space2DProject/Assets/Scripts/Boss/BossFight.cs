using UnityEngine;

public class BossFight : MonoBehaviour
{
    private bool isInBossFight;
    
    private GameObject bossRoom;
    private GameObject bossStartRoom;
    private GameObject bossObj;
    private Transform startPos;
    public GameObject playerFollower;

    void Start()
    {
        bossRoom = transform.GetChild(0).gameObject;
        bossObj = bossRoom.transform.GetChild(2).gameObject;
        bossStartRoom = transform.GetChild(1).gameObject;
        startPos = bossStartRoom.transform.GetChild(2);
    }
    
    void Update()
    {
        
    }
    
    public void ActivateBossFight(bool yes = true)
    {
        bossRoom.transform.parent = yes ? LevelManager.Instance.Level().GetChild(0) : transform;
        bossRoom.SetActive(yes);
        bossStartRoom.transform.parent = yes ? LevelManager.Instance.Level().GetChild(0) : transform;
        bossStartRoom.SetActive(yes);
        playerFollower.SetActive(!yes);
        
    }

    public Transform SpawnBossAndReturnStartPos()
    {
        bossObj.SetActive(true);
        return startPos;
    }

    public void CancelBossFight()
    {
        ActivateBossFight(false);
        bossObj.SetActive(false);
        //+reset boss stats
    }

    public bool IsInBossFight()
    {
        return isInBossFight;
    }

    public void StartBossFight()
    {
        isInBossFight = true;
    }
}
