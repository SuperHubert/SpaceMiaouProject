using System.Collections;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public GameObject currentFollower;
    public GameObject fallAnimObj;
    private Animator fallAnim;
    private FollowPlayer follow;
    private PlayerMovement playerMovement;

    public float teleportCooldown = 1f;
    
    private Transform pointBotLeft;
    private Transform pointTopRight;
    private Transform playerTransform;
    private GameObject playerObj;
    private bool canFall = true;

    public Coroutine teleportFollowerRoutine;
    
    void Start()
    {
        pointBotLeft = transform.GetChild(0);
        pointTopRight = transform.GetChild(1);
        follow = currentFollower.GetComponent<FollowPlayer>();
        playerTransform = transform.parent;
        playerObj = playerTransform.gameObject;
        playerMovement = playerObj.GetComponent<PlayerMovement>();
        fallAnim = fallAnimObj.GetComponent<Animator>();
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer != 13) return;
        if (!other.OverlapPoint(pointBotLeft.position) || !other.OverlapPoint(pointTopRight.position) || !canFall) return;
        if (playerMovement.dashing) return;
        canFall = false;
        
        fallAnimObj.transform.position = transform.position;
        fallAnim.Play("Noyade");
        InputManager.canInput = false;
        playerTransform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = playerObj.GetComponent<SpriteRenderer>().enabled = LifeManager.Instance.canTakeDamge = false;
        StartCoroutine(DoTheFalling());
    }

    IEnumerator DoTheFalling()
    {
        yield return new WaitForSeconds(2.5f);
        playerTransform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = playerObj.GetComponent<SpriteRenderer>().enabled = LifeManager.Instance.canTakeDamge = true;
        LifeManager.Instance.TakeDamages(1);
        playerTransform.position = currentFollower.transform.position;
        canFall = true;
        InputManager.canInput = true;
    }

    public IEnumerator TeleportFollower(bool instant = false)
    {
        if(instant)yield return null;
        follow.WarpToPlayer();
        follow.canMove = true;
    }

    public void ResetFollowerPos()
    {
        StartCoroutine(TeleportFollower());
    }
}
