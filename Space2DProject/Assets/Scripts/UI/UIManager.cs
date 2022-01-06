using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<GameObject> HpList;
    private List<Animator> HpAnims = new List<Animator>();
    public GameObject normalUI;


    #region Singleton

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        foreach (var obj in HpList)
        {
            HpAnims.Add(obj.GetComponent<Animator>());
        }
        

    }

    public void SetHpUI(int number, bool hpGain)
    {
        for (int i = 0; i < 9; i++)
        {
            if (i < number)
            {
                if (hpGain)
                {
                    HpAnims[i].Rebind();
                    HpAnims[i].Update(0f);
                }
            }
            else
            {
                HpAnims[i].SetTrigger("Off");
            }
        }
    }
}