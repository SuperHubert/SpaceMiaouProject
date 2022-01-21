using System.Collections;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public GameObject currentFollower;
    public GameObject fallAnimObj;
    private Animator fallAnim;
    private FollowPlayer follow;
    private PlayerMovement playerMovement;
    private LifeManager lifeManager;
    
    private Transform pointBotLeft;
    private Transform pointTopRight;
    private Transform playerTransform;
    private GameObject playerObj;
    private bool canFall = true;
    
    private AudioManager am;
    
    void Start()
    {
        pointBotLeft = transform.GetChild(0);
        pointTopRight = transform.GetChild(1);
        follow = currentFollower.GetComponent<FollowPlayer>();
        playerTransform = transform.parent;
        playerObj = playerTransform.gameObject;
        playerMovement = playerObj.GetComponent<PlayerMovement>();
        fallAnim = fallAnimObj.GetComponent<Animator>();
        lifeManager =LifeManager.Instance;
        am = AudioManager.Instance;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer != 13) return;
        if (!other.OverlapPoint(pointBotLeft.position) || !other.OverlapPoint(pointTopRight.position) || !canFall) return;
        if (playerMovement.dashing) return;
        StartCoroutine(DoTheFalling());
    }
    
    IEnumerator DoTheFalling()
    {
        LevelManager.Instance.DisablePlayer(1f);
        fallAnimObj.transform.position = transform.position;
        am.Play(18, true);
        fallAnim.Play("Noyade");
        yield return new WaitForSeconds(1f);
        playerTransform.position = currentFollower.transform.position;
        lifeManager.TakeDamages(1);
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
