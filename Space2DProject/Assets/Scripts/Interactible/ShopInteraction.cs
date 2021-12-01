using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInteraction : MonoBehaviour, IInteractible
{
    public GameObject shopUI;
    public ShopManager shopManager;
    public Button exitButton;


    public void OnInteraction()
    {
        shopUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        Time.timeScale = 1;
    }
}
