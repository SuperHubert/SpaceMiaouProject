using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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
        if(transform.position == player.position || SceneManager.GetActiveScene().buildIndex == 3) return;
        agent.Warp(player.position);
    }

}
