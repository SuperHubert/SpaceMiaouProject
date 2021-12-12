using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopManager : MonoBehaviour
{
    public int reductionTotal = 0;

    #region Singleton
    public static ShopManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [System.Serializable] public class ShopItem
    {
        public string name;
        public Sprite Image;
        public int basePrice;
        public int actualPrice;
        public UnityEvent upgrade = new UnityEvent();
        public bool isBought = false;

    }

    [SerializeField] List<ShopItem> ShopItemList = new List<ShopItem>();
    [SerializeField] List<ShopItem> ShopItemList2 = new List<ShopItem>();

     public void ReductionPickI()
    {
        reductionTotal ++;
    }

    public void ReductionPickII()
    {
        reductionTotal += 2;
    }

    public List<ShopItem> DisplayItems(int floor, int seed, int size)
    {
        ShopItemList2.Clear();
        foreach(var item in ShopItemList)
        {
            ShopItemList2.Add(item);
        }


        List<ShopItem> returnList = new List<ShopItem>();

        Random.InitState(seed + floor);

        for (int i = 0; i < floor+1; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (ShopItemList2.Count <= 0)
                {
                    foreach (var item in ShopItemList)
                    {
                        ShopItemList2.Add(item);
                    }

                }

                var addedItem = ShopItemList2[Random.Range(0, ShopItemList2.Count)];

                returnList.Add(addedItem);

                ShopItemList2.Remove(addedItem);
            }

            if(i != floor)
            {
                returnList.Clear();
            }
            
        }

        return returnList;
    }
    
}
