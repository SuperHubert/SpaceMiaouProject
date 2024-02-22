using TMPro;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private AudioManager am;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score : "+LoadingLevelData.Instance.score;
        AudioManager.Instance.Play(25, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
