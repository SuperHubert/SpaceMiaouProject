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

    public void PlayGame()
    {
        LoadingManager.Instance.LoadScene(3);
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
}
