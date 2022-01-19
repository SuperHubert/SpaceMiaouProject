using TMPro;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private AudioManager am;

    void Start()
    {
        scoreText.text = "Score : "+LoadingLevelData.Instance.score;
        AudioManager.Instance.Play(25, true);
    }
}
