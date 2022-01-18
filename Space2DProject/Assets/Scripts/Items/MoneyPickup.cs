using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    void OnTriggerEnter2D (Collider2D other)
    {
        if (!other.transform.CompareTag("Coin")) return;
        Destroy(other.gameObject);
        MoneyManager.Instance.PickupCoin();
    }
}
