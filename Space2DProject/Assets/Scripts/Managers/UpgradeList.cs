using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeList : MonoBehaviour
{
    [SerializeField] private List<GameObject> upgradeList = new List<GameObject>();
    [SerializeField] private List<int> upgradePrice = new List<int>();

    #region Singleton
    public static UpgradeList Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<GameObject> GetUpgradeList()
    {
        return upgradeList;

    }

    public List<int> GetUpgradePrice()
    {
        return upgradePrice;
    }

}
