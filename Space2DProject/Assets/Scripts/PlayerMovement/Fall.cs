using System.Collections;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public GameObject currentFollower;
    private FollowPlayer follow;
    private PlayerMovement playerMovement;
    
    private Transform pointBotLeft;
    private Transform pointTopRight;
    private bool canFall = true;

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
        DoTheFalling();
    }

    public void DoTheFalling()
    {
        transform.parent.position = currentFollower.transform.position;
        canFall = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StartCoroutine(TeleportFollower());
    }

    public IEnumerator TeleportFollower()
    {
        yield return null;
        follow.WarpToPlayer();
        follow.canMove = true;
    }
}
