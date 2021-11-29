using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    private bool isInBossFight;
    
    private GameObject bossRoom;
    private GameObject bossStartRoom;
    private Transform startPos;
    
    void Start()
    {
        bossRoom = transform.GetChild(0).gameObject;
        bossStartRoom = transform.GetChild(1).gameObject;
        startPos = bossStartRoom.transform.GetChild(2);
    }
    
    void Update()
    {
        
    }
    
    public void ActivateBossFight(bool yes = true)
    {
        StartCoroutine(FakeLoading());
        bossRoom.SetActive(yes);
        bossStartRoom.SetActive(yes);
        LevelManager.Instance.MovePlayer(startPos);
    }
    private IEnumerator FakeLoading(float frames = 60f)
    {
        for (var i = 0; i < frames; i++)
        {
            LoadingManager.Instance.UpdateLoading(i/frames);
            yield return null;
        }
        LoadingManager.Instance.UpdateLoading(2);
    }
    
    public void CancelBossFight(bool yes = true)
    {
        ActivateBossFight(false);
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
