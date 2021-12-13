using System.Collections;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public GameObject currentFollower;
    private FollowPlayer follow;
    private PlayerMovement playerMovement;

    public float teleportCooldown = 1f;
    
    private Transform pointBotLeft;
    private Transform pointTopRight;
    private bool canFall = true;

    public Coroutine teleportFollowerRoutine;
    
    void Start()
    {
        pointBotLeft = transform.GetChild(0);
        pointTopRight = transform.GetChild(1);
        follow = currentFollower.GetComponent<FollowPlayer>();
        playerMovement = transform.parent.gameObject.GetComponent<PlayerMovement>();
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer != 13) return;
        if (!other.OverlapPoint(pointBotLeft.position) || !other.OverlapPoint(pointTopRight.position) || !canFall) return;
        if (playerMovement.dashing) return;
        canFall = false;
        StartCoroutine(DoTheFalling());
    }

    IEnumerator DoTheFalling()
    {
        //play Animation;
        InputManager.canInput = false;
        yield return new WaitForSeconds(1f);
        LifeManager.Instance.TakeDamages(1);
        transform.parent.position = currentFollower.transform.position;
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
