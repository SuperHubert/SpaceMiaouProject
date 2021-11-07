using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> lootTable = new List<GameObject>();
    private GameObject coin;
    
    #region Singleton

    public static LootManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        coin = lootTable[0];
    }

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

    public void GetCoins(int numberOfCoins,Vector3 pos)
    {
        Debug.Log(numberOfCoins);
        for (int i = 0; i < numberOfCoins; i++)
        {
            Debug.Log("coin");
            Instantiate(coin,pos, quaternion.identity);
        }
        
        
    }
}
