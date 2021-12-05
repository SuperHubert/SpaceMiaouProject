using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    [SerializeField] private GameObject tpTarget;

    private Transform player;
    private Transform pointBotLeft;
    private Transform pointTopRight;
    private bool canFall = true;

    void Start()
    {
        pointBotLeft = transform.GetChild(0);
        pointTopRight = transform.GetChild(1);
        player = transform.parent;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer != 13) return;
        if (!other.OverlapPoint(pointBotLeft.position) || !other.OverlapPoint(pointTopRight.position) || !canFall) return;
        canFall = false;
        DoTheFalling();
        Debug.Log("Overlap");
    }

    public void DoTheFalling()
    {
        Debug.Log("fall");
        player.position = tpTarget.transform.position;
        canFall = true;
    }
}
