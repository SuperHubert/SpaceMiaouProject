using System.Collections;
using UnityEngine;

public class RubikaLogo : MonoBehaviour
{
    public GameObject logo;
    public GameObject buttons;
    
    void Start()
    {
        if(LoadingLevelData.Instance.noMoreLogo) return;
        LoadingLevelData.Instance.noMoreLogo = true;
        StartCoroutine(RemoveLogo());
    }

    IEnumerator RemoveLogo()
    {
        buttons.SetActive(false);
        logo.SetActive(true);
        yield return new WaitForSeconds(1);
        logo.SetActive(false);
        yield return new WaitForSeconds(1);
        buttons.SetActive(true);
    }
    
}
