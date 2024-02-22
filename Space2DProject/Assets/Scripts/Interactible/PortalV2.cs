using System.Collections;
using UnityEngine;

public class PortalV2 : MonoBehaviour, IInteractible
{
    [SerializeField] private FollowPlayer followPlayer;
    [SerializeField] private ShopInteraction shopInteraction;
    private Animator animator;
    private Transform tpPos;
    private AudioManager am;
    private LevelManager lm;

    private void Start()
    {
        tpPos = transform.GetChild(1);
        animator = gameObject.GetComponent<Animator>();
        am = AudioManager.Instance;
        lm = LevelManager.Instance;
    }

    public void OnInteraction()
    {
        DialogueManager.Instance.EndDialogue();
        
        shopInteraction.displayList.Clear();
        
        animator.SetTrigger("Trigger");

        lm.DisablePlayer(1.22f);
        
        StartCoroutine(AnimationRoutine());
    }

    IEnumerator AnimationRoutine()
    {
        am.Play(31, true);

        lm.MovePlayer(tpPos);
        
        tpPos.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.22f);
        lm.Player().SetActive(false);

        LoadingManager.Instance.UpdateLoading();
        lm.GenerateNextLevel();
    }
}