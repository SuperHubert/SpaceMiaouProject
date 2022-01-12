using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingManager : MonoBehaviour
{
    public GameObject canvas;
    private Image backgroundImage;
    private Image progressBar;
    public Animator animator;
    public List<RuntimeAnimatorController> controllersReserve;
    [SerializeField] private List<RuntimeAnimatorController> controllers = new List<RuntimeAnimatorController>();


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
        RefillControllers();
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
            RotationItem();
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

    public void RotationItem()
    {
        if(controllers.Count == 0) RefillControllers();
        RuntimeAnimatorController controller = controllers[Random.Range(0, controllers.Count)];
        Debug.Log(controller);
        animator.runtimeAnimatorController = controller;
        controllers.Remove(controller);
    }

    private void RefillControllers()
    {
        controllers.Clear();
        foreach (var controller in controllersReserve)
        {
            controllers.Add(controller);
        }
    }
}
