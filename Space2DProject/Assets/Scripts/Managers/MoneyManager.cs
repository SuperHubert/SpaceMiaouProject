using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int nyanCoins = 0;
    [SerializeField] private Animator coinAnim;
    [SerializeField] private TextMeshProUGUI nyanCount;
    
    private static readonly int GainPick = Animator.StringToHash("Trigger");

    #region Singleton
    public static MoneyManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    public void PickupCoin()
    {
        nyanCoins ++;
        nyanCount.text = nyanCoins.ToString();
        
        coinAnim.SetTrigger(GainPick);
    }

    public void SetCoins(int number)
    {
        nyanCoins = number;
        nyanCount.text = nyanCoins.ToString();
    }

 
    
}
