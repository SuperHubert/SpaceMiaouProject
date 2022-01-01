using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopInteraction : MonoBehaviour, IInteractible
{
    public GameObject shopUI;
    public ShopManager shopManager;
    public Button selectedButton;
    public TextMeshProUGUI nyanCountShop;
    public TextMeshProUGUI nyanCount;
    public Animator animator;

    public List<TextMeshProUGUI> textList;
    public List<Image> imageList;
    public List<TextMeshProUGUI> testNameList;
    public List<GameObject> soldOutList;

    public List<ShopManager.ShopItem> displayList = new List<ShopManager.ShopItem>();

    private bool canOpenShop = true;
    public bool closeShopInput = false;
    private static readonly int PickUpAnimation = Animator.StringToHash("PickUpAnimation");


    private void Update()
    {
        if (!closeShopInput || !shopUI.activeSelf) return;

        CloseShop();

    }
    
    private void RefreshShop()
    {
        if (displayList.Count == 0)
        {
            displayList = shopManager.SetNewDisplayItems(LevelManager.Instance.GetCurrentFloorNumber(), LevelManager.Instance.GetFirstSeed(), 5);
        }

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

            imageList[i].sprite = displayList[i].image;
        }
    }
    

    private bool CanBuy(int index)
    {
        if (index < 0 || index >= displayList.Count) return false;
        if (displayList[index].isBought) return false;
        return displayList[index].actualPrice <= MoneyManager.Instance.nyanCoins;
    }

    IEnumerator InteractionCooldown()
    {
        yield return new WaitForSeconds(0.05f);
        canOpenShop = true;
    }

    IEnumerator InteractionAnimation()
    {
        canOpenShop = false;
        LevelManager.Instance.Player().SetActive(false);
        Vector3 pos = transform.position;
        Debug.Log(pos);
        LevelManager.Instance.Player().transform.position = new Vector3(pos.x+0.126f,pos.y-0.594f,0);
        InputManager.canInput = false;
        animator.SetTrigger(PickUpAnimation);
        yield return new WaitForSeconds(1.05f);
        InputManager.canInput = true;
        shopUI.SetActive(true);
        selectedButton.Select();
        Time.timeScale = 0;

    }
    
    public void OnInteraction()
    {
        if(shopUI.activeSelf || !canOpenShop) return;
        
        StartCoroutine(InteractionAnimation());

        RefreshShop();
    }

    public void CloseShop()
    {
        LevelManager.Instance.Player().SetActive(true);
        shopUI.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine(InteractionCooldown());
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
        nyanCount.text = MoneyManager.Instance.nyanCoins.ToString();
        RefreshShop();
    }

    public void UpdateAppearance()
    {
        var biome = LevelManager.Instance.GetBiome();
        
        switch (biome)
        {
            case 0:
                var floorNumber = LevelManager.Instance.GetCurrentFloorNumber();
                gameObject.SetActive((floorNumber != 0));
                if (floorNumber != 0) return;
                
                animator.SetLayerWeight (2, 0f);
                animator.SetLayerWeight (1, 1f);
                break;
            case 1:
                gameObject.SetActive(true);
                animator.SetLayerWeight (2, 1f);
                animator.SetLayerWeight (1, 0f);
                break;
            case 2:
                gameObject.SetActive(true);
                animator.SetLayerWeight (2, 0f);
                animator.SetLayerWeight (1, 1f);
                break;
            default:
                gameObject.SetActive(true);
                animator.SetLayerWeight (2, 0f);
                animator.SetLayerWeight (1, 1f);
                break;
        }

    }
}
