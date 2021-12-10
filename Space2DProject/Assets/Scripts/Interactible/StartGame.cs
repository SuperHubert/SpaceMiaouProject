using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour, IInteractible
{
    public void OnInteraction()
    {
        LoadingManager.Instance.UpdateLoading(0.01f);
        
        SceneManager.LoadScene(4);
    }
}
