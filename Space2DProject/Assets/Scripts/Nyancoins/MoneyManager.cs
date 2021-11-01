using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public float nyanCoins = 0;
    public Animator coinAnim;


    #region Singleton
    public static MoneyManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

}
