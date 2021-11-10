using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour, IInteractible
{
   [SerializeField] private GameObject shop;
  public void OnInteraction()
    {
        shop.SetActive(true);
        Time.timeScale = 0;
    }
}
