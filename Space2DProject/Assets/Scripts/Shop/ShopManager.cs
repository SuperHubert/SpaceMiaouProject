using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        public Sprite image;
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

    public List<ShopItem> SetNewDisplayItems(int floor, int seed, int size = 5)
    {
        ShopItemList2.Clear();
        foreach (var newItem in ShopItemList.Select(item => new ShopItem
        {
            name = item.name,
            image = item.image,
            basePrice = item.basePrice,
            actualPrice = item.actualPrice,
            upgrade = item.upgrade,
            isBought = item.isBought
        }))
        {
            ShopItemList2.Add(newItem);
        }


        List<ShopItem> returnList = new List<ShopItem>();
        
        for (int i = 0; i < floor+1; i++)
        {
            Random.InitState(seed);
            for (int j = 0; j < size; j++)
            {
                if (ShopItemList2.Count <= 0)
                {
                    foreach (var newItem in ShopItemList.Select(item => new ShopItem
                    {
                        name = item.name,
                        image = item.image,
                        basePrice = item.basePrice,
                        actualPrice = item.actualPrice,
                        upgrade = item.upgrade,
                        isBought = item.isBought
                    }))
                    {
                        ShopItemList2.Add(newItem);
                    }
                }


                var addedItem = ShopItemList2[Random.Range(0, ShopItemList2.Count)];
                returnList.Add(addedItem);
                ShopItemList2.Remove(addedItem);

            }

            string text = "";

            foreach (var item in returnList)
            {
                text += item.name + " ";
            }
            
            Debug.Log("Floor "+i+" : "+text);

            if (i != floor)
            {
                returnList.Clear();
            }
        }

        var healItem = new ShopItem
        {
            name = ShopItemList[0].name,
            image = ShopItemList[0].image,
            basePrice = ShopItemList[0].basePrice,
            actualPrice = ShopItemList[0].actualPrice,
            upgrade = ShopItemList[0].upgrade,
            isBought = ShopItemList[0].isBought
        };
        
        returnList.Add(healItem);
        
        return returnList;
    }
    
}
