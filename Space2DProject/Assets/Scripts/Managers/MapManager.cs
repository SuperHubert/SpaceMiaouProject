using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject normalUI;
    [SerializeField] private GameObject mapUI;
    private bool fullIsOn = false;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            fullIsOn = !fullIsOn;
            if (fullIsOn)
            {
                normalUI.SetActive(false);
                mapUI.SetActive(true);
            }
            else
            {
                normalUI.SetActive(true);
                mapUI.SetActive(false);
            }
            
        }
    }
}
