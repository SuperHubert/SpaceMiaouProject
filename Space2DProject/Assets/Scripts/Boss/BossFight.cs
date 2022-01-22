using UnityEngine;
using UnityEngine.UI;

public class BossFight : MonoBehaviour
{
    private bool isInBossFight;
    
    private GameObject bossRoom;
    private GameObject bossStartRoom;
    private GameObject bossObj;
    private Transform startPos;
    public GameObject playerFollower;
    public GameObject mapHider;
    public GameObject mapHiderMinimap;

    void Start()
    {
        bossRoom = transform.GetChild(0).gameObject;
        bossObj = bossRoom.transform.GetChild(2).gameObject;
        bossStartRoom = transform.GetChild(1).gameObject;
        startPos = bossStartRoom.transform.GetChild(2);
        bossStartRoom.SetActive(false);
        bossRoom.SetActive(false);
    }
    
    public void ActivateBossFight(bool yes = true)
    {
        Transform bossHealth = bossRoom.transform.GetChild(2).GetChild(0).GetChild(1);
        bossHealth.parent = yes ? LevelManager.Instance.Level().GetChild(5) : bossHealth.parent;
        bossHealth.localPosition = new Vector3(0,-320,0);
        var healthBar = bossHealth.GetComponent<Image>();
        healthBar.rectTransform.localScale = Vector3.one;
        healthBar.rectTransform.sizeDelta = new Vector2(810, 30);

        LevelManager.Instance.Level().GetChild(6).position = new Vector3(-5,-40,0);
        LevelManager.Instance.Level().GetChild(3).position = new Vector3(0,50,0);
        LevelManager.Instance.Level().GetChild(7).position = new Vector3(0,50,0);
        
        bossRoom.transform.parent = yes ? LevelManager.Instance.Level().GetChild(0) : transform;
        bossRoom.SetActive(yes);
        bossStartRoom.transform.parent = yes ? LevelManager.Instance.Level().GetChild(0) : transform;
        bossStartRoom.SetActive(yes);
        playerFollower.SetActive(!yes);
        mapHider.SetActive(yes);
        mapHiderMinimap.SetActive(yes);
        
        isInBossFight = yes;

    }

    public Transform SpawnBossAndReturnStartPos()
    {
        
        return startPos;
    }

    public void SpawnBoss()
    {
        bossObj.SetActive(true);
        bossObj.transform.GetChild(0).GetComponent<EnemyHealth>().InitEnemy();
    }

    public void CancelBossFight()
    {
        ActivateBossFight(false);
        bossObj.transform.GetChild(0).GetComponent<EnemyHealth>().InitEnemy();
        bossObj.SetActive(false);
        //+reset boss stats
    }

    public bool IsInBossFight()
    {
        return isInBossFight;
    }
}
