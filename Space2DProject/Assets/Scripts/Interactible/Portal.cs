using System;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractible
{
    [SerializeField] private FollowPlayer followPlayer;
    [SerializeField] private ShopInteraction shopInteraction;
    [SerializeField] private GameObject rayObj;

    public void OnInteraction()
    {
        followPlayer.canMove = false;
        
        rayObj.SetActive(true);
        
        DialogueManager.Instance.EndDialogue();
        
        shopInteraction.displayList.Clear();
    }
}
