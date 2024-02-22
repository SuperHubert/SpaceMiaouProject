using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject normalUI;
    [SerializeField] private GameObject mapUI;
    private bool bigMapIsActive = false;

    public bool mapInput = false;
    public bool mapExitInput = false;
    
    void Update()
    {
        if ((mapInput || (bigMapIsActive && mapExitInput)) && !ShopManager.Instance.shopCanvas.activeSelf)
        {
            //if (LevelManager.Instance.IsInBossFight()) return;
            DisplayMap();
        }
    }

    private void DisplayMap()
    {
        bigMapIsActive = !bigMapIsActive;
        if (bigMapIsActive)
        {
            normalUI.SetActive(false);
            mapUI.SetActive(true);
            Time.timeScale = 0;

        }
        else
        {
            normalUI.SetActive(true);
            mapUI.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
