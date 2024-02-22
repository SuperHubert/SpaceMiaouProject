using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnRestartRun : MonoBehaviour
{
    
    void Start()
    {
        if(!LoadingLevelData.firstRun) Destroy(gameObject);
    }
}
