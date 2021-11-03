using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour, IInteractible
{
    public void OnInteraction()
    {
        Debug.Log("AAAAAAAAAAA");
        SceneManager.LoadScene(1);
        return;
    }
}
