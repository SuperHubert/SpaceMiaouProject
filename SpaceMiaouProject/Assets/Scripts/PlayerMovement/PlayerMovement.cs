using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public float speed = 10f;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        rb.MovePosition(player.transform.position + new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, Input.GetAxis("Vertical") * speed * Time.deltaTime,0));
    }
}
