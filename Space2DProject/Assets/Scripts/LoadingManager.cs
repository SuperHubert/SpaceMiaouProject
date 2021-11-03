using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image progressBar;

    #region Singleton Don't Destroy On Load
    public static LoadingManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    #endregion

    public async void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(1);
        canvas.SetActive(true);
        var scene = SceneManager.LoadSceneAsync(sceneNumber);
        scene.allowSceneActivation = false;

        do
        {
            progressBar.fillAmount = scene.progress;
        } while (scene.progress < 0.9f);
        
        scene.allowSceneActivation = true;
        canvas.SetActive(false);
    }
}
