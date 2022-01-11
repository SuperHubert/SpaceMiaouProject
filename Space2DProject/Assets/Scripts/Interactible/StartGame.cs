using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject player;
    [SerializeField] GameObject rayObj;
    [SerializeField] Transform tpPos;
    [SerializeField] Transform parent;
    
    public void OnInteraction()
    {
        if(parent != null) transform.SetParent(parent);
        StartCoroutine(AnimationRoutine());
    }

    IEnumerator AnimationRoutine()
    {
        player.transform.position = tpPos.transform.position;
        player.SetActive(false);
        tpPos.gameObject.SetActive(true);
        rayObj.SetActive(true);
        
        yield return new WaitForSeconds(0.11f);
        
        tpPos.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.22f);
        
        rayObj.SetActive(false);
        LoadingManager.Instance.canvas.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        SceneManager.LoadScene(4);
    }
}
