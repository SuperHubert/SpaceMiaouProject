using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopInteraction : MonoBehaviour, IInteractible
{
    public GameObject shopUI;
    public ShopManager shopManager;
    public Button selectedButton;
    private GameObject previousSelectedObj;
    public TextMeshProUGUI nyanCountShop;
    public TextMeshProUGUI nyanCount;
    public Animator animator;
    
    public List<TextMeshProUGUI> textList;
    public List<Image> imageList;
    public List<GameObject> buttonList = new List<GameObject>();
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Image itemImage;

    public TextMeshProUGUI inventoryText;
    public List<ShopManager.ShopItem> inventory;

    public Sprite baseButtonSprite;
    public Sprite soldOutSprite;

    public List<ShopManager.ShopItem> displayList = new List<ShopManager.ShopItem>();

    private AudioManager am = AudioManager.Instance;

    public List<Dialogues> shopDialogues;

    private bool canOpenShop = true;
    public bool closeShopInput = false;
    private static readonly int PickUpAnimation = Animator.StringToHash("PickUpAnimation");
    

    private void Update()
    {
        if (!shopUI.activeSelf) return;
        
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (previousSelectedObj == null)
            {
                selectedButton.Select();;
            }
            else
            {
                previousSelectedObj.GetComponent<Button>().Select();
            }
            
        }
        else if (EventSystem.current.currentSelectedGameObject != previousSelectedObj)
        {
            previousSelectedObj = EventSystem.current.currentSelectedGameObject;
            UpdateDescription();
        }

        if (!closeShopInput) return;
        CloseShop();

    }
    
    private void RefreshShop()
    {
        if (displayList.Count == 0)
        {
            displayList = shopManager.SetNewDisplayItems(LevelManager.Instance.GetCurrentFloorNumber(), LevelManager.Instance.GetFirstSeed(), 5);
            Debug.Log("Updated shop");
        }

        for (int i = 0; i < textList.Count; i++)
        {
            displayList[i].actualPrice = displayList[i].basePrice - shopManager.reductionTotal;
            if(displayList[i].actualPrice <= 0) 
            { 
                displayList[i].actualPrice = 1; 
            }
            textList[i].text = displayList[i].actualPrice.ToString();
            buttonList[i].GetComponent<Image>().sprite = baseButtonSprite;
            
            if (displayList[i].isBought)
            {
                buttonList[i].GetComponent<Image>().sprite = soldOutSprite;
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
        UIManager.Instance.normalUI.SetActive(false);
        Vector3 pos = transform.position;
        LevelManager.Instance.Player().transform.position = new Vector3(pos.x+0.126f,pos.y-0.594f,0);
        InputManager.canInput = false;
        animator.SetTrigger(PickUpAnimation);
        yield return new WaitForSeconds(1.05f);
        InputManager.canInput = true;
        shopUI.SetActive(true);
        selectedButton.Select();
        Time.timeScale = 0;

    }

    private void UpdateDescription()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i] != previousSelectedObj) continue;
            itemName.text = displayList[i].name;
            itemDescription.text = displayList[i].description;
            itemImage.sprite = displayList[i].image;
        }
    }

    private void UpdateInventory()
    {
        string returnText = "";

        if (inventory.Count == 0) returnText = "(No Upgrades)";
        foreach (var item in inventory)
        {
            returnText += "-" + item.name + "\n";
        }

        inventoryText.text = returnText;

    }

    public void OnInteraction()
    {
        if (LoadingLevelData.shopDialogue)
        {
            DialogueManager.Instance.StartMultipleDialogues(shopDialogues,false);
            StartCoroutine(WaitForDialogueToEnd());
        }
        else
        {
            OpenShop();
        }
    }

    IEnumerator WaitForDialogueToEnd()
    {
        yield return new WaitUntil(() => !DialogueManager.Instance.dialogueCanvas.activeSelf);
        OpenShop();
    }

    private void OpenShop()
    {
        if(shopUI.activeSelf || !canOpenShop) return;
        
        StartCoroutine(InteractionAnimation());

        am.Play(19, true);

        RefreshShop();
    }

    public void CloseShop()
    {
        LevelManager.Instance.Player().SetActive(true);
        UIManager.Instance.normalUI.SetActive(true);
        shopUI.SetActive(false);
        UpdateInventory();
        Time.timeScale = 1;
        StartCoroutine(InteractionCooldown());
    }
    
    public void Button(int index)
    {
        if (!CanBuy(index)) return;
        MoneyManager.Instance.nyanCoins -= displayList[index].actualPrice;
        am.Play(10, true);
        displayList[index].isBought = true;
        if(displayList[index].track) inventory.Add(displayList[index]);
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
