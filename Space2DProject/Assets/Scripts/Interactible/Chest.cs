using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour, IInteractible
{
    private float[] table = {100f,0f};
    private int floor;
    public static float fortuneUpgrade = 0;
    

    private void Start()
    {
        floor = LevelManager.Instance.FloorNumber();
    }

    public void OnInteraction()
    {
        float drop = Random.Range(0, 100);
        if (drop < LootManager.Instance.ConvertLevelToProbability(floor))
        {
            LootManager.Instance.GetCoins(floor > 2 ? Random.Range(1, floor + 1) : Random.Range(1, 3),
                transform.position);
                
            Destroy(gameObject);
        }
        else
        {
            LootManager.Instance.GetUpgrade(floor,transform.position);
            
            Destroy(gameObject);
        }
    }

    public void UpdateChest()
    {
        float probabilityToStay = ((floor + (4f + 5 * fortuneUpgrade)) / (floor + (5f + 5 * fortuneUpgrade))) * 0.5f;
        
        if (Random.Range(0f, 1f) > probabilityToStay)
        {
            Destroy(gameObject);
        }
    }

    public void AddFortune(float f)
    {
        fortuneUpgrade += f;
    }
}
