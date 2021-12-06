using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    
    public void Init()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        player = LevelManager.Instance.Player().transform;
    }
    
    void Update()
    {
        if(!InputManager.canInput) return;
        //agent.SetDestination(player.position);
    }
    
}
