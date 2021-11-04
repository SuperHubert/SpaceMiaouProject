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

    public void LoadScene(int sceneNumber)
    {
        canvas.SetActive(true);
        
        StartCoroutine(LoadAsynchronously(sceneNumber));
    }

    IEnumerator LoadAsynchronously(int sceneNumber)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneNumber);
        
        while (!scene.isDone)
        {
            float progress = Mathf.Clamp01(scene.progress / 0.9f);
            
            progressBar.fillAmount = progress;
            
            yield return null;
        }
        
        canvas.SetActive(false);
    }
}
