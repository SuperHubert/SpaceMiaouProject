using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LifeManager : MonoBehaviour
{
    public int lifeBar = 9;
    public int damagesPepito = 1;
    public int maxHP = 9;

    public void TestDamages()
    {
        TakeDamages(damagesPepito);
        
    }
    
   public void TakeDamages(int damages)
    {
        int previousHP = lifeBar;
        lifeBar -= damages;
        if (lifeBar > maxHP)
        {
            lifeBar = maxHP;
        }
        else if (lifeBar < 0)
        {
            lifeBar = 0;
        }
        UIManager.Instance.UpdateHpUI(previousHP, lifeBar);

    } 


}
