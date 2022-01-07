using System;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractible
{
    [SerializeField] private FollowPlayer followPlayer;
    [SerializeField] private ShopInteraction shopInteraction;

    public void OnInteraction()
    {
        followPlayer.canMove = false;
        
        DialogueManager.Instance.EndDialogue();
        
        shopInteraction.displayList.Clear();
        
        LoadingManager.Instance.UpdateLoading();
        
        LevelManager.Instance.GenerateNextLevel();
    }
}
