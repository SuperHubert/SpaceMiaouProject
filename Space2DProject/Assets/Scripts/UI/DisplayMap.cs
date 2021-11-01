using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMap : MonoBehaviour
{
    [SerializeField] private GameObject map;
    private bool isActive = false;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isActive = !isActive;
            map.SetActive(isActive);
        }
    }
}
