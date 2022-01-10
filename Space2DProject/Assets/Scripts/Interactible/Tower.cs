using UnityEngine;

public class Tower : MonoBehaviour,IInteractible
{
    private GameObject laserObj;
    private Collider2D portalCollider2D;
    
    private void Start()
    {
        laserObj = transform.GetChild(0).gameObject;
        portalCollider2D = transform.parent.GetChild(3).GetComponent<Collider2D>();
    }

    public void OnInteraction()
    {
        laserObj.SetActive(true);
        portalCollider2D.enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        KillAllEnemies();
        
    }

    void KillAllEnemies()
    {
        foreach (Transform enemy in transform.parent.GetChild(1))
        {
            enemy.GetChild(0).GetComponent<EnemyHealth>().Die(false);
            Destroy(enemy.gameObject,2.5f);
            
        }
    }
}
