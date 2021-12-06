using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        player = LevelManager.Instance.Player().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //agent.SetDestination(player.position);
    }
    
}
