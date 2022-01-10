using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public GameObject canvas;
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
            return;
        }
        
    }
    #endregion

    private void Start()
    {
        backgroundImage = canvas.transform.GetChild(0).gameObject.GetComponent<Image>();
        progressBar = canvas.transform.GetChild(1).gameObject.GetComponent<Image>();
        loadingText = canvas.transform.GetChild(2).gameObject;
    }

    public void LoadScene(int sceneNumber)
    {
        if (showCanvas)
        {
            canvas.SetActive(true);
            InputManager.canInput = false;
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
        InputManager.canInput = true;
        progressBar.fillAmount = 0;
    }

    void ChangeBools(bool canvas,bool image,bool progress)
    {
        showCanvas = canvas;
        backgroundImage.gameObject.SetActive(image);
        showProgress = progress;
        progressBar.gameObject.SetActive(progress);
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
            InputManager.canInput = true;
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
            InputManager.canInput = true;
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
                ChangeBools(true, true, false);
                break;

            default:
                break;
        }

        return mode;
    }

    
}
