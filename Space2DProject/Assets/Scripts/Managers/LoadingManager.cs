using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private Image backgroundImage;
    private Image progressBar;
    private GameObject loadingText;

    private bool showCanvas = true;
    private bool showImage = true;
    private bool showProgress = true;
    
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

    private void Start()
    {
        //backgroundImage = canvas.transform.GetChild(0).GetComponent<Image>;
    }

    public void LoadScene(int sceneNumber)
    {
        if (showCanvas)
        {
            canvas.SetActive(true);
        }

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
        progressBar.fillAmount = 0;
    }

    void ChangeBools(bool canvas,bool image,bool progress)
    {
        showCanvas = canvas;
        showImage = image;
        showProgress = progress;
    }

    public void UpdateLoading(float progress = 0)
    {
        if (!showCanvas)
        {
            return;
        }

        if (progress > 1)
        {
            canvas.SetActive(false);
            return;
        }
        
        if (canvas.activeSelf)
        {
            if (showProgress)
            {
                progressBar.fillAmount = progress;
            }
        }
        else
        {
            canvas.SetActive(true);
        }
    }

    public int ChangeLoadingMode(int mode)
    {
        switch (mode)
        {
            case 0:
                ChangeBools(true, true, true);
                break;

            case 1:
                ChangeBools(false, true, true);
                break;

            case 2:
                ChangeBools(true, false, true);
                break;

            case 3:
                ChangeBools(true, false, true);
                break;

            default:
                break;
        }

        return mode;
    }

    
}
