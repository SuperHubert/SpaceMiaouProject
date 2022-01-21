using System.Collections;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D col;
    private GameObject player;
    private LifeManager lifeManager;

    private void Start()
    {
        player = LevelManager.Instance.Player();
        lifeManager = LifeManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(LifeManager.Instance.canTakeDamge) StartCoroutine(GrabAnimation());
    }

    IEnumerator GrabAnimation()
    {
        LevelManager.Instance.DisablePlayer(1);
        col.enabled = false;
        animator.SetTrigger("Trigger");
        yield return new WaitForSeconds(1f);
        player.transform.position = transform.position + new Vector3(0.111f,-0.817f,0);
        lifeManager.TakeDamages(1);
        yield return new WaitForSeconds(1f);
        col.enabled = true;

    }
}
