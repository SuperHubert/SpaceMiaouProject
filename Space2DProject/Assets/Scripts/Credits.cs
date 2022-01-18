using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject movingCredits;
    [SerializeField] private GameObject staticCredits;
    [SerializeField] private GameObject thanks;
    private AudioManager am;
    
    void Start()
    {
        AudioManager.Instance.Play(24);

        if (LoadingLevelData.creditsGoToMenu)
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
        
        if (thanks.activeSelf || LoadingLevelData.creditsGoToMenu)
        {
            SceneManager.LoadScene(LoadingLevelData.creditsGoToMenu ? 2 : 3);
        }
    }
}
