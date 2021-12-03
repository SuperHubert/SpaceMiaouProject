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

    public List<TextMeshProUGUI> textList;
    public List<TextMeshProUGUI> testNameList;

    public void OnInteraction()
    {
        shopUI.SetActive(true);
        Time.timeScale = 0;

        var displayList = shopManager.DisplayItems(0,1,5);

        //var displayList = shopManager.DisplayItems(LevelManager.Instance.GetCurrentFloorNumber(), LevelManager.Instance.GetCurrentSeed(), 5);

        for (int i = 0; i < textList.Count; i++)
        {
            displayList[i].actualPrice = displayList[i].basePrice - shopManager.reductionTotal;
            textList[i].text = displayList[i].actualPrice.ToString();
            testNameList[i].text = displayList[i].name;

        }
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        Time.timeScale = 1;
    }

    
}
