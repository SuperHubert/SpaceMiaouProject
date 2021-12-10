using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    public bool canMove = false;
    public bool isInHub = true;
    
    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        canMove = false;
    }
    
    void Update()
    {
        if(!InputManager.canInput || agent == null || !canMove || isInHub) return;
        if(transform.position == player.position) return;
        //agent.Warp(player.position);
    }

    public void WarpToPlayer()
    {
        if(transform.position == player.position) return;
        agent.Warp(player.position);
    }

}
