using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int nyanCoins = 0;
    [SerializeField] private Image movingCoin;
    [SerializeField] private Animator coinAnim;
    [SerializeField] private TextMeshProUGUI nyanCount;
  


    public Transform playerTransform;

    private static readonly int GainPick = Animator.StringToHash("GainPick");

    #region Singleton
    public static MoneyManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    void Start()
    {
        //coinAnim = movingCoin.GetComponent<Animator>();
    }
    
    public void PickupCoin()
    {
        nyanCoins ++;
        Debug.Log("moula");
        nyanCount.text = nyanCoins.ToString();
        
        coinAnim.SetTrigger(GainPick);
    }

 
    
}
