using TMPro;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score : "+LoadingLevelData.Instance.score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
