using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ManagerMenu : MonoBehaviour
{
    [SerializeField] private Button firstSelectedButton;
    
    private GameObject previousSelectedObj;
    private void Start()
    {
        firstSelectedButton.Select();
    }
    
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (previousSelectedObj == null)
            {
                firstSelectedButton.Select();;
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
        if (LoadingLevelData.Instance.hasLaunchedGame)
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
}
