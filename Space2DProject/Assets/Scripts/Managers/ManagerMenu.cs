using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ManagerMenu : MonoBehaviour
{
    [SerializeField] private Button firstSelectedButton;
    public GameObject previousSelectedObj;
    
    [SerializeField] private GameObject inputImage;
    [SerializeField] private Slider soundSlider;

    [SerializeField] private GameObject pauseUI;
    private void Start()
    {
        firstSelectedButton.Select();
    }
    
    private void Update()
    {
        if(soundSlider != null) return;
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (previousSelectedObj == null)
            {
                firstSelectedButton.Select();
            }
            else
            {
                previousSelectedObj.GetComponent<Button>().Select();
            }
            
        }
        else if (EventSystem.current.currentSelectedGameObject != previousSelectedObj)
        {
            previousSelectedObj = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void PlayGame()
    {
        LoadingLevelData.creditsGoToMenu = false;
        LoadingManager.Instance.RotationItem();
        if (LoadingLevelData.skipCinematic)
        {
            LoadingManager.Instance.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(6);
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitter");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene(2);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void DisplayInput(bool forceOff = false)
    {
        if (forceOff)
        {
            inputImage.SetActive(false);
            if(pauseUI != null) pauseUI.SetActive(true);
        }
        else
        {
            if(pauseUI != null) pauseUI.SetActive(false);
            inputImage.SetActive(!inputImage.activeSelf);
        }
    }

    public void GoToCredits()
    {
        LoadingLevelData.creditsGoToMenu = true;
        SceneManager.LoadScene(7);
    }

    public void GoToMenuFromGame()
    {
        LifeManager.Instance.CleanReturnToMenu();
    }

    public void ChangeVolume()
    {
        AudioManager.Instance.ChangeVolume(soundSlider.value);
    }
}
