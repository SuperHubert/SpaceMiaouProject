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
        am = AudioManager.Instance;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer != 13) return;
        if (!other.OverlapPoint(pointBotLeft.position) || !other.OverlapPoint(pointTopRight.position) || !canFall) return;
        if (playerMovement.dashing) return;
        canFall = false;
        InputManager.canInput = false;
        InputManager.canMove = false;
        
        
        fallAnimObj.transform.position = transform.position;
        am.Play(18, true);
        fallAnim.Play("Noyade");
        
        playerTransform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = playerObj.GetComponent<SpriteRenderer>().enabled = LifeManager.Instance.canTakeDamge = false;
        StartCoroutine(DoTheFalling());
    }

    IEnumerator DoTheFalling()
    {
        fallAnim.Play("Noyade");
        yield return new WaitForSeconds(2.5f);
        playerTransform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = playerObj.GetComponent<SpriteRenderer>().enabled = LifeManager.Instance.canTakeDamge = true;
        LifeManager.Instance.TakeDamages(1);
        playerTransform.position = currentFollower.transform.position;
        canFall = true;
        InputManager.canInput = true;
        InputManager.canMove = true;
        playerObj.SetActive(true);
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
