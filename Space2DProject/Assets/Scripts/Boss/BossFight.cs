using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    private bool isInBossFight;
    
    private GameObject bossRoom;
    private GameObject bossStartRoom;
    private GameObject bossObj;
    private Transform startPos;
    
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
        bossRoom.SetActive(yes);
        bossStartRoom.SetActive(yes);
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
