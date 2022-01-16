using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject movingCredits;
    [SerializeField] private GameObject staticCredits;
    [SerializeField] private GameObject thanks;
    
    void Start()
    {
        if (LoadingLevelData.Instance.creditsGoToMenu)
        {
            movingCredits.SetActive(false);
            staticCredits.SetActive(true);
        }
        else
        {
            movingCredits.SetActive(true);
            staticCredits.SetActive(false);
        }
        
    }
    
    void Update()
    {
        ReturnToHub();
    }

    private void ReturnToHub()
    {
        if (!Input.anyKeyDown) return;
        
        if (thanks.activeSelf || LoadingLevelData.Instance.creditsGoToMenu)
        {
            SceneManager.LoadScene(LoadingLevelData.Instance.creditsGoToMenu ? 2 : 3);
        }
    }
}
