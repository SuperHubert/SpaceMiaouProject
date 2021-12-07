using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public static GameObject currentFollower;
    
    private Transform pointBotLeft;
    private Transform pointTopRight;
    private bool canFall = true;

    void Start()
    {
        pointBotLeft = transform.GetChild(0);
        pointTopRight = transform.GetChild(1);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer != 13) return;
        if (!other.OverlapPoint(pointBotLeft.position) || !other.OverlapPoint(pointTopRight.position) || !canFall) return;
        canFall = false;
        DoTheFalling();
    }

    public void DoTheFalling()
    {
        //player.position = currentFollower.transform.position;
        canFall = true;
    }
}
