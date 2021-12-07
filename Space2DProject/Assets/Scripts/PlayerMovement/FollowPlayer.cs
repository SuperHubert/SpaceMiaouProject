using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    public bool canMove = false;

    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        player = LevelManager.Instance.Player().transform;
        Fall.currentFollower = gameObject;
        canMove = false;
    }

    public void Init()
    {
        canMove = false;
    }
    
    void Update()
    {
        if(!InputManager.canInput || agent == null || !canMove) return;
        agent.SetDestination(player.position);
    }
    
}
