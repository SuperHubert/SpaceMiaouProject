using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BossCinematic : MonoBehaviour
{
    public Camera cam;
    public BossFight bossFight;
    private Transform camTransform;
    public GameObject tower;
    public Transform door;
    public CameraManager camManager;
    public GameObject boss;
    private Animator bossAnimator;
    
    public Dialogues dialogue01;
    public Dialogues dialogue02;
    public Dialogues dialogue03;

    private AudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        camTransform = cam.transform;
        bossAnimator = boss.GetComponent<Animator>();
        am = AudioManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        UIManager.Instance.normalUI.SetActive(false);
        InputManager.canInput = false;
        camManager.enabled = false;
        camTransform.DOMove(new Vector3(0, -5.76f, -10), 2.5f);
        cam.DOOrthoSize(12, 2.5f);
        yield return new WaitForSeconds(2.5f);
        
        DialogueManager.Instance.StartDialogue(dialogue01);
        yield return new WaitUntil(() => !DialogueManager.Instance.dialogueCanvas.activeSelf);
        am.Stop(8, true);
        tower.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(2f);
        
        cam.DOShakePosition(2f, Vector3.one);
        yield return new WaitForSeconds(2f);
        
        camTransform.DOMove(new Vector3(0, -9, -10), 2.5f);
        cam.DOOrthoSize(7, 2.5f);
        DialogueManager.Instance.StartDialogue(dialogue02);
        yield return new WaitForSeconds(2.5f);
        
        yield return new WaitUntil(() => !DialogueManager.Instance.dialogueCanvas.activeSelf);
        camTransform.DOMove(new Vector3(0, -1.6f, -10), 1f);
        bossFight.SpawnBoss();
        DialogueManager.Instance.StartDialogue(dialogue03);
        yield return new WaitUntil(() => !DialogueManager.Instance.dialogueCanvas.activeSelf);
        am.Play(3, true);
        StartCoroutine(LevelManager.Instance.LatePlay(9, 1));

        bossAnimator.enabled = true;
        yield return new WaitForSeconds(1f);
        camManager.enabled = true;
        InputManager.canInput = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        UIManager.Instance.normalUI.SetActive(true);
        LevelManager.Instance.Player().transform.position = new Vector3(0,-12,0);
        foreach (Transform child in door)
        {
            child.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        bossAnimator.SetTrigger("Idle");
    }
}
