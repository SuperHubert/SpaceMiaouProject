using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject movingCredits;
    [SerializeField] private GameObject staticCredits;
    [SerializeField] private GameObject score;
    private TextMeshProUGUI scoreText;
    
    void Start()
    {
        if (LoadingLevelData.creditsGoToMenu)
        {
            movingCredits.SetActive(false);
            staticCredits.SetActive(true);
        }
        else
        {
            movingCredits.SetActive(true);
            staticCredits.SetActive(false);
            AudioManager.Instance.Play(24);
        }

        scoreText = score.GetComponent<TextMeshProUGUI>();
        scoreText.text = "Final Score : "+ LoadingLevelData.Instance.score;
        LoadingLevelData.Instance.score = 0;
    }
    
    void Update()
    {
        ReturnToHub();
    }

    private void ReturnToHub()
    {
        if (!Input.anyKeyDown) return;

        if (!score.activeSelf && !LoadingLevelData.creditsGoToMenu) return;
        
        AudioManager.Instance.Stop(24,true);
        SceneManager.LoadScene(LoadingLevelData.creditsGoToMenu ? 2 : 3);
    }
}
