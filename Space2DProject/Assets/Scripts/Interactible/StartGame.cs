using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject player;
    [SerializeField] private Animator animator;
    [SerializeField] Transform parent;
    
    public void OnInteraction()
    {
        if(parent != null) transform.SetParent(parent);
        animator.SetTrigger("Trigger");
        StartCoroutine(AnimationRoutine());
    }

    public float value;
    IEnumerator AnimationRoutine()
    {
        player.SetActive(false);
        yield return new WaitForSeconds(1f);
        LoadingManager.Instance.canvas.SetActive(true);
        //SceneManager.LoadScene(4);
    }
}
