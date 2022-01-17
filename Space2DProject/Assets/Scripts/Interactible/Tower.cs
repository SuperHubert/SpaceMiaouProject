using UnityEngine;
using DG.Tweening;

public class Tower : MonoBehaviour,IInteractible
{
    private GameObject laserObj;
    private Collider2D portalCollider2D;
    public Camera cam;
    
    private void Start()
    {
        laserObj = transform.GetChild(0).gameObject;
        portalCollider2D = transform.parent.GetChild(3).GetComponent<Collider2D>();
    }
    
    public void OnInteraction()
    {
        AudioManager.Instance.Play(17);
        cam.DOShakePosition(1f,new Vector3(0.1f,0.4f,0),8,0,true);
        laserObj.SetActive(true);
        portalCollider2D.enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        KillAllEnemies();
        
    }

    void KillAllEnemies()
    {
        foreach (Transform item in transform.parent.GetChild(2))
        {
            if (item.GetComponent<Arena>() != null) item.GetComponent<Arena>().completed = true;
            if (item.GetComponent<Chest>() == null) continue;
            item.GetComponent<Chest>().OnInteraction();
            item.gameObject.SetActive(false);
        }
        
        
        foreach (Transform enemy in transform.parent.GetChild(1))
        {
            if (!enemy.gameObject.activeSelf) continue;
            if (enemy.GetChild(0).gameObject.activeSelf)
            {
                enemy.GetChild(0).GetComponent<EnemyHealth>().Die(false);
            }

        }
    }
}
