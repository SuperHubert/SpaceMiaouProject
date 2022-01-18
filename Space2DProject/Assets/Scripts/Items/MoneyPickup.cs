using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class MoneyPickup : MonoBehaviour
{
    void OnTriggerEnter2D (Collider2D other)
    {
        if(other.transform.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            AudioManager.Instance.Play(32, true);
            MoneyManager.Instance.PickupCoin();
        }
    }
}
