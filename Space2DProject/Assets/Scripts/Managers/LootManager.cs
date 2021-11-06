using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> lootTable = new List<GameObject>();
    
    #region Singleton

    public static LootManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public void GetLootTable(int level)
    {
        
    }

    public float ConvertLevelToProbability(int level)
    {
        if (level < 4)
        {
            return 80;
        }
        
        float probability = (1f / ((level / (level + 1f)) * (4f / 3f))) * 80;

        return probability;
    }
}
