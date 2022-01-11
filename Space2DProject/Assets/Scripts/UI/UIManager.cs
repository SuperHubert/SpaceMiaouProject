using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<GameObject> HpList;
    private List<Animator> HpAnims = new List<Animator>();
    
    public GameObject normalUI;
    public GameObject pauseUI;
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

    private void Start()
    {
        foreach (var obj in HpList)
        {
            HpAnims.Add(obj.GetComponent<Animator>());
        }
    }

    private void Update()
    {
        Pause();
    }

    public void SetHpUI(int number, bool hpGain)
    {
        for (int i = 0; i < 9; i++)
        {
            if (i < number)
            {
                if (hpGain)
                {
                    HpAnims[i].Rebind();
                    HpAnims[i].Update(0f);
                }
            }
            else
            {
                HpAnims[i].SetTrigger("Off");
            }
        }
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
            Time.timeScale = 1;
        }
    }
}