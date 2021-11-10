using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
  [System.Serializable] class ShopItem
    {
        public Sprite Image;
        public int Price;
        public bool Purchased = false;
    }

    [SerializeField] List<ShopItem> shopItemList;
    private List<GameObject> upgradeList = new List<GameObject>();
    private List<int> upgradePrice = new List<int>();

    private void Start()
    {
        upgradeList = UpgradeList.Instance.GetUpgradeList();
        upgradePrice = UpgradeList.Instance.GetUpgradePrice();
    }



    public void CloseShop()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }


}
