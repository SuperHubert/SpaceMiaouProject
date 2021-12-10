using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractible
{
    [SerializeField] private FollowPlayer followPlayer;
    
    public void OnInteraction()
    {
        followPlayer.canMove = false;
        
        LoadingManager.Instance.UpdateLoading();
        
        LevelManager.Instance.GenerateNextLevel();
    }
}
