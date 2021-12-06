using UnityEngine;


public class LifeManager : MonoBehaviour
{
    public int lifeBar = 9;
    public int damagesPepito = 1;
    public int maxHP = 9;

    #region Singleton
    public static LifeManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    public void TestDamages()
    {
        TakeDamages(damagesPepito);
        
    }
    
   public void TakeDamages(int damages)
    {
        var previousHp = lifeBar;
        lifeBar -= damages;
        if (lifeBar > maxHP)
        {
            lifeBar = maxHP;
        }
        else if (lifeBar < 0)
        {
            lifeBar = 0;
        }
        UIManager.Instance.UpdateHpUI(previousHp, lifeBar);

    } 


}
