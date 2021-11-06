using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour, IInteractible
{
    public void OnInteraction()
    {
        LoadingManager.Instance.UpdateLoading();
        
        SceneManager.LoadScene(4);
    }
}