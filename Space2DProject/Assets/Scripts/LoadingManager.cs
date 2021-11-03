using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Image progressBar;

    #region Singleton
    public static LoadingManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Start()
    {
        StartCoroutine(LoadAsyncOperation(4));
    }
    
    IEnumerator LoadAsyncOperation(int sceneNumber)
    {
        AsyncOperation level = SceneManager.LoadSceneAsync(sceneNumber);

        while (level.progress < 1)
        {
            progressBar.fillAmount = level.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
