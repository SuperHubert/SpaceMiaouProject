using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalV2 : MonoBehaviour, IInteractible
{
    [SerializeField] private FollowPlayer followPlayer;
    [SerializeField] private ShopInteraction shopInteraction;
    private GameObject rayObj;
    private Transform tpPos;

    private void Start()
    {
        rayObj = transform.GetChild(0).gameObject;
        tpPos = transform.GetChild(1);
    }

    public void OnInteraction()
    {
        followPlayer.canMove = false;
        
        DialogueManager.Instance.EndDialogue();
        
        shopInteraction.displayList.Clear();

        StartCoroutine(AnimationRoutine());
    }

    IEnumerator AnimationRoutine()
    {
        LevelManager.Instance.MovePlayer(tpPos);
        LevelManager.Instance.Player().SetActive(false);
        tpPos.gameObject.SetActive(true);
        rayObj.SetActive(true);
        
        yield return new WaitForSeconds(0.11f);
        
        tpPos.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.22f);
        
        rayObj.SetActive(false);
        LevelManager.Instance.Player().SetActive(true);
        LoadingManager.Instance.UpdateLoading();
        LevelManager.Instance.GenerateNextLevel();
    }
}