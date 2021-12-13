using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerMenu : MonoBehaviour
{
    [SerializeField] private Button firstSelectedButton;
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
}
