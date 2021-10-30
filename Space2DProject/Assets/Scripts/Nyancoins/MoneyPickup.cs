using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyPickup : MonoBehaviour
{
    public TextMeshProUGUI nyanCount;
    public Image coin;
    public Animator coinAnim;

    void Start()
    {
        coinAnim = coin.GetComponent<Animator>();
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if(other.transform.tag == "Coin")
        {
            Destroy(other.gameObject);
            MoneyManager.Instance.nyanCoins ++;
            Debug.Log("moula");
            nyanCount.text = MoneyManager.Instance.nyanCoins.ToString();
            coinAnim.SetTrigger("GainPick");
        }
    }
}
