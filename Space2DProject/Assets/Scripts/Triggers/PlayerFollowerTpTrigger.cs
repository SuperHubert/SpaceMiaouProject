using System;
using System.Collections;
using UnityEngine;

public class PlayerFollowerTpTrigger : MonoBehaviour
{
    private Fall fall;

    private void Start()
    {
        fall = LevelManager.Instance.Player().GetComponent<PlayerMovement>().fall;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        fall.ResetFollowerPos();
    }
}
