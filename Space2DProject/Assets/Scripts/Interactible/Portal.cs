using UnityEngine;

public class Portal : MonoBehaviour, IInteractible
{
    [SerializeField] private FollowPlayer followPlayer;
    [SerializeField] private ShopInteraction shopInteraction;
    [SerializeField] private DialogueManager dialogueManager;
    
    public void OnInteraction()
    {
        followPlayer.canMove = false;
        
        dialogueManager.EndDialogue();
        
        shopInteraction.displayList.Clear();
        
        LoadingManager.Instance.UpdateLoading();
        
        LevelManager.Instance.GenerateNextLevel();
    }
}
