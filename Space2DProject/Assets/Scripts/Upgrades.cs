using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [SerializeField] private GameObject chestobj;
    private Chest chest;
    [SerializeField] private bool isChest = true;
    public static float fortuneUpgrade = 0;
    
    #region Singleton
    public static Upgrades Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Start()
    {
        if (!isChest) return;
        chest = chestobj.GetComponent<Chest>();
    }

    public void Upgrade1()
    {
        Debug.Log("Got Upgrade 1");    
    }
    
    public void Upgrade2()
    {
        Debug.Log("Got Upgrade 2");    
    }
    
    public void Upgrade3()
    {
        Debug.Log("Got Upgrade 3");    
    }

    public void Heal1()
    {
        LifeManager.Instance.TakeDamages(-1);
        Debug.Log("Got Heal I");
    }

    public void Heal2()
    {
        LifeManager.Instance.TakeDamages(-2);
        Debug.Log("Got Heal II");
    }

    public void Heal3()
    {
        LifeManager.Instance.TakeDamages(-3);
        Debug.Log("Got Heal III");
    }

    public void Speed1()
    {
        var levelm = LevelManager.Instance.Player().GetComponent<PlayerMovement>();
        levelm.speed += levelm.speed * 5f/100f;
        Debug.Log("Got Speed I");
    }

    public void Speed2()
    {
        var levelm = LevelManager.Instance.Player().GetComponent<PlayerMovement>();
        levelm.speed += levelm.speed * 10f/100f;
        Debug.Log("Got Speed II");
    }

    public void Speed3()
    {
        var levelm = LevelManager.Instance.Player().GetComponent<PlayerMovement>();
        levelm.speed += levelm.speed * 15f/100f;
        Debug.Log("Got Speed III");
    }

    public void DamagesBalai1()
    {
        var combat = LevelManager.Instance.Player().GetComponent<Combat2>();
        combat.damage += combat.damage * 5f / 100f;
        Debug.Log("Damages Balai I");
    }

    public void DamagesBalai2()
    {
        var combat = LevelManager.Instance.Player().GetComponent<Combat2>();
        combat.damage += combat.damage * 10f / 100f;
        Debug.Log("Damages Balai II");
    }

    public void DamagesBalai3()
    {
        var combat = LevelManager.Instance.Player().GetComponent<Combat2>();
        combat.damage += combat.damage * 15f / 100f;
        Debug.Log("Damages Balai III");
    }

    public void RechargeDash1()
    {
        LevelManager.Instance.Player().GetComponent<PlayerMovement>().dashCdMax -= LevelManager.Instance.Player().GetComponent<PlayerMovement>().dashCdMax * 5f / 100f;
        Debug.Log("Recharge Dash I");
    }

    public void RechargeDash2()
    {
        LevelManager.Instance.Player().GetComponent<PlayerMovement>().dashCdMax -= LevelManager.Instance.Player().GetComponent<PlayerMovement>().dashCdMax * 10f / 100f;
        Debug.Log("Recharge Dash II");
    }

    public void RechargeDash3()
    {
        LevelManager.Instance.Player().GetComponent<PlayerMovement>().dashCdMax -= LevelManager.Instance.Player().GetComponent<PlayerMovement>().dashCdMax * 15f / 100f;
        Debug.Log("Recharge Dash III");
    }

    public void Fortune1()
    {
        Debug.Log("Fortune I");
        fortuneUpgrade += 1;
    }

    public void Fortune2()
    {
        Debug.Log("Fortune II");
        fortuneUpgrade += 2;
    }

    public void Fortune3()
    {
        Debug.Log("Fortune III");
        fortuneUpgrade += 3;
    }

    public void Javel1()
    {
        var sprayAttack = LevelManager.Instance.Player().GetComponent<SprayAttack>();
        if (sprayAttack.burn)
        {
            sprayAttack.burnDamage += 0.01f;
        }
        else
        {
            sprayAttack.burn = true; 
        }
        
        Debug.Log("Eau de Javel I");
    }

    public void DamagesSpray1()
    {
        var sprayAttack = LevelManager.Instance.Player().GetComponent<SprayAttack>();
        sprayAttack.damage += sprayAttack.damage * 5f / 100f;
        Debug.Log("Damages Spray I");
    }

    public void DamagesSpray2()
    {
        var sprayAttack = LevelManager.Instance.Player().GetComponent<SprayAttack>();
        sprayAttack.damage += sprayAttack.damage * 10f / 100f;
        Debug.Log("Damages Spray II");
    }

    public void Reductions1()
    {
        Debug.Log("Reductions I");
        ShopManager.Instance.ReductionPickI();
    }

    public void Reductions2()
    {
        Debug.Log("Reductions II");
        ShopManager.Instance.ReductionPickII();
    }

    public void SoapAmmo1()
    {
        var combat = LevelManager.Instance.Player().GetComponent<Combat2>();
        combat.sprayGainNormal += combat.sprayGainNormal * 5f / 100f;
        combat.sprayGainSpecial += combat.sprayGainSpecial * 5f / 100f;
        Debug.Log("Recharge de Savon I");
    }

    public void SoapAmmo2()
    {
        var combat = LevelManager.Instance.Player().GetComponent<Combat2>();
        combat.sprayGainNormal += combat.sprayGainNormal * 10f / 100f;
        combat.sprayGainSpecial += combat.sprayGainSpecial * 10f / 100f;
        Debug.Log("Recharge de Savon II");
    }

    public float GetFortuneLevel()
    {
        return fortuneUpgrade;
    }
}
