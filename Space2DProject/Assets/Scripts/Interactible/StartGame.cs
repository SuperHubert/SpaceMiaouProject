using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject player;
    [SerializeField] private Animator animator;
    [SerializeField] Transform parent;
    public Dialogues dialogue;
    
    public void OnInteraction()
    {
        if(parent != null) transform.SetParent(parent);
        StartCoroutine(AnimationRoutine());
    }

    public float value;
    IEnumerator AnimationRoutine()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
        InputManager.canInput = false;
        yield return new WaitUntil(() => !DialogueManager.Instance.dialogueCanvas.activeSelf);
        player.SetActive(false);
        animator.SetTrigger("Trigger");
        yield return new WaitForSeconds(1.22f);
        LoadingManager.Instance.canvas.SetActive(true);
        yield return null;
        SceneManager.LoadScene(4);
    }
}
