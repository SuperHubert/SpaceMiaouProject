using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<GameObject> HpList;


    #region Singleton
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public void UpdateHpUI(int pvAvant,int pvApres)
    {
        Debug.Log(pvAvant + " " + pvApres);

        if (!(pvAvant == pvApres))
        {
            if (pvAvant > pvApres)
            {

                for (int i = pvAvant - 1; i > pvApres - 1; i--)
                {
                    HpList[i].GetComponent<Animator>().SetTrigger("Off");
                }
            }
            else
            {
                for (int i = pvAvant; i < pvApres; i++)
                {
                    HpList[i].GetComponent<Animator>().SetTrigger("On");
                }
            }
        }
        
    }

}
