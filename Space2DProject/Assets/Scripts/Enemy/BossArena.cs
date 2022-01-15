using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArena : MonoBehaviour
{
    [SerializeField] private NewBossBehaviour bossBehaviour;
    
    void Update()
    {
        if (!CheckIfKidsAreDead()) return;
        bossBehaviour.ExitArenaMode();
        gameObject.SetActive(false);
    }

    private bool CheckIfKidsAreDead()
    {
        bool returnValue = true;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf) returnValue = false;
        }

        return returnValue;
    }
}
