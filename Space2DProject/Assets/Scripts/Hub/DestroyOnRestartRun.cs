using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnRestartRun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(LoadingLevelData.Instance.firstRunDialogue) Destroy(gameObject);
    }
}
