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
        float drop = Random.Range(0, 100);
        if (drop < LootManager.Instance.ConvertLevelToProbability(LevelManager.Instance.FloorNumber()))
        {
            Debug.Log("Dropped Coins");
        }
        else
        {
            Debug.Log("Dropped Upgrade");
        }
    }
}
