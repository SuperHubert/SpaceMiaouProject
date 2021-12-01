using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
        [SerializeField] private UnityEvent upgrade = new UnityEvent();
        public Sprite Image;
        public int basePrice;
        private int actualPrice;
    }

    [SerializeField] List<ShopItem> ShopItemList;
    List<ShopItem> ShopItemList2;

    private void Start()
    {
        DuplicateList();
    }

    public void DuplicateList()
    {
        ShopItemList2 = ShopItemList;
    }



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
