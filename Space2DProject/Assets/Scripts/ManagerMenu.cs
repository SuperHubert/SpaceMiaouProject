using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //SceneManager.LoadScene(x);
        Debug.Log("Jouer");
    }

    public void QuitGame()
    {
        //Application.Quit(x);
        Debug.Log("Quitter");
    }

    public void OpenSettings()
    {
        //SceneManager.LoadScene(x);
        Debug.Log("Parametres");
    }
}
