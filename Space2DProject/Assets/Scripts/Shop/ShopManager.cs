using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public float reductionTotal = 0;

  [System.Serializable] class ShopItem
    {
        public Sprite Image;
        public int Price;
        public bool Purchased = false;
    }

    [SerializeField] List<ShopItem> ShopItemList;

}
