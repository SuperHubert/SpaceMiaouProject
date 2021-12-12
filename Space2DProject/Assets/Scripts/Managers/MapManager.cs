using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject normalUI;
    [SerializeField] private GameObject mapUI;
    private bool bigMapIsActive = false;

    public bool mapInput = false;
    
    void Update()
    {
        if (!mapInput) return;
        
        bigMapIsActive = !bigMapIsActive;
        if (bigMapIsActive)
        {
            normalUI.SetActive(false);
            mapUI.SetActive(true);
            Time.timeScale = 0;

        }
        else
        {
            normalUI.SetActive(true);
            mapUI.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
