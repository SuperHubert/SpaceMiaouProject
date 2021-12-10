using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopInteraction : MonoBehaviour, IInteractible
{
    public GameObject shopUI;
    public ShopManager shopManager;
    public Button exitButton;
    public TextMeshProUGUI nyanCountShop;

    public List<TextMeshProUGUI> textList;
    public List<TextMeshProUGUI> testNameList;
    public List<GameObject> soldOutList;

    public List<ShopManager.ShopItem> displayList = new List<ShopManager.ShopItem>();
    

    public void OnInteraction()
    {
        shopUI.SetActive(true);
        Time.timeScale = 0;

        displayList.Clear();
        //displayList = shopManager.DisplayItems(0,1,5);

        displayList = shopManager.DisplayItems(LevelManager.Instance.GetCurrentFloorNumber(), LevelManager.Instance.GetCurrentSeed(), 5);

        for (int i = 0; i < textList.Count; i++)
        {
            displayList[i].actualPrice = displayList[i].basePrice - shopManager.reductionTotal;
            if(displayList[i].actualPrice <= 0) 
            { 
                displayList[i].actualPrice = 1; 
            }
            textList[i].text = displayList[i].actualPrice.ToString();
            testNameList[i].text = displayList[i].name;
            soldOutList[i].SetActive(false);
            if (displayList[i].isBought)
            {
                soldOutList[i].SetActive(true);
            }
            nyanCountShop.text = MoneyManager.Instance.nyanCoins.ToString();

        }
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        Time.timeScale = 1;
    }

  
    public void ButtonHeal()
    {
        //Heal
        Debug.Log("Heal");
    }

    public void Button(int index)
    {
        if (!CanBuy(index)) return;
        Debug.Log("item bought)");
        MoneyManager.Instance.nyanCoins -= displayList[index].actualPrice;
        displayList[index].isBought = true;
        soldOutList[index].SetActive(true);
        displayList[index].upgrade.Invoke();
        nyanCountShop.text = MoneyManager.Instance.nyanCoins.ToString();

    }

    private bool CanBuy(int index)
    {
        if (index < 0 || index >= displayList.Count) return false;
        if (displayList[index].isBought) return false;
        if (displayList[index].actualPrice <= MoneyManager.Instance.nyanCoins) return true;
        return false;
        
    }

}
