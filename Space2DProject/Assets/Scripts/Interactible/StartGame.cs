using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour, IInteractible
{
    public void OnInteraction()
    {
        StartCoroutine(StartTheGameDelay());
    }

    private IEnumerator StartTheGameDelay()
    {
        LoadingManager.Instance.UpdateLoading();
        yield return new WaitForSeconds(0.05f);
        SceneManager.LoadScene(4);
    }
}
