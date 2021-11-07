using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour, IInteractible
{
    private float[] table = {100f,0f};
    

    private void Start()
    {
        
    }

    public void OnInteraction()
    {
        int floor = LevelManager.Instance.FloorNumber();
        float drop = Random.Range(0, 100);
        if (drop < LootManager.Instance.ConvertLevelToProbability(floor))
        {
            LootManager.Instance.GetCoins(floor > 2 ? Random.Range(1, floor + 1) : Random.Range(1, 3),
                transform.position);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Dropped Upgrade");
        }
    }
}
