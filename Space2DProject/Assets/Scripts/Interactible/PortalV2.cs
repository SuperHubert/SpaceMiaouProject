using System.Collections;
using UnityEngine;

public class PortalV2 : MonoBehaviour, IInteractible
{
    [SerializeField] private FollowPlayer followPlayer;
    [SerializeField] private ShopInteraction shopInteraction;
    private Animator animator;
    private Transform tpPos;
    private AudioManager am;

    private void Start()
    {
        tpPos = transform.GetChild(1);
        animator = gameObject.GetComponent<Animator>();
        am = AudioManager.Instance;
    }

    public void OnInteraction()
    {
        followPlayer.canMove = false;


        
        DialogueManager.Instance.EndDialogue();
        
        shopInteraction.displayList.Clear();
        
        animator.SetTrigger("Trigger");

        StartCoroutine(AnimationRoutine());
    }

    IEnumerator AnimationRoutine()
    {
        am.Play(31, true);

        LevelManager.Instance.MovePlayer(tpPos);
        LevelManager.Instance.Player().SetActive(false);
        tpPos.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(1.22f);
        
        LoadingManager.Instance.UpdateLoading();
        LevelManager.Instance.GenerateNextLevel();
        LevelManager.Instance.Player().SetActive(true);
    }
}