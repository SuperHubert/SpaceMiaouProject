using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public float reductionTotal = 0;

    #region Singleton
    public static ShopManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [System.Serializable] class ShopItem
    {
        public Sprite Image;
        public int Price;
        public bool Purchased = false;
    }

    [SerializeField] List<ShopItem> ShopItemList;


    public void ReductionPickI()
    {
        reductionTotal ++;
        Debug.Log("Reduction +1");

    }

    public void ReductionPickII()
    {
        reductionTotal++;
        Debug.Log("Reduction +2");

    }
}
