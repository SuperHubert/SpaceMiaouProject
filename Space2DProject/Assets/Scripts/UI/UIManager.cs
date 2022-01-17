using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image hpImage;
    public Image hpImageBack;
    
    public GameObject normalUI;
    public GameObject pauseUI;
    public GameObject inputImage;
    public bool pauseInput;

    public int score;
    public TextMeshProUGUI scoreText;


    #region Singleton

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
    

    private void Update()
    {
        Pause();
        if (Math.Abs(hpImage.fillAmount - hpImageBack.fillAmount) > 0.0001f)
        {
            HealthDecreaseEffect();
        }
        
    }
    
    public void UpdateHp(float progress)
    {
        hpImage.fillAmount = progress;
    }

    public void IncreaseScore(int number)
    {
        score += number;
        if (number == 0) score = 0;
        scoreText.text = "Score : " + score;
    }

    public void Pause(bool force = false)
    {
        if (!pauseInput && !force) return;
        pauseUI.SetActive(!pauseUI.activeSelf);
        if (pauseUI.activeSelf)
        {
            pauseUI.transform.GetChild(1).GetChild(0).GetComponent<Button>().Select();
            Time.timeScale = 0;
        }
        else
        {
            inputImage.SetActive(false);
            Time.timeScale = 1;
        }
    }
    
    private void HealthDecreaseEffect()
    {
        if(hpImage != null) hpImageBack.fillAmount = Mathf.Lerp (hpImageBack.fillAmount, hpImage.fillAmount, 3f * Time.deltaTime);
    }
}