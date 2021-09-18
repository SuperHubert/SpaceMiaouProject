using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;

/// <summary>
/// La classe qui gere les mouvement du joueur.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    //Player GameObject
    public GameObject player;
    private Rigidbody rb;
    
    //normal movement
    public float speed = 10f;
    
    //dash related
    public float dashSpeed = 40f;
    
    bool dashing = false;
    bool dashKeyPressed = false;
    bool canDash = true;
    
    [SerializeField] float dashInternalCd;
    public float dashInternalCdMax = 2f;
    [SerializeField] float dashCd;
    public float dashCdMax = 1f;
    
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        dashInternalCd = 0;
        dashCd = 0;

    }

    private void Update()
    {
        dashKeyPressed = Input.GetButtonDown("Dash");
        
        if (dashKeyPressed && canDash)
        {
            dashing = true;
            canDash = false;
            dashCd = 0;
        }

        if (dashing)
        {
            if (dashInternalCd >= dashInternalCdMax)
            {
                dashing = false;
                dashInternalCd = 0;
            }
            else
            {
                dashInternalCd += Time.deltaTime;
            }
        }

        if (!canDash)
        {
            if (dashCd >= dashCdMax)
            {
                canDash = true;
                dashCd = 0;
            }
            else
            {
                dashCd += Time.deltaTime;
            }
        }
        
    }

    void FixedUpdate()
    { 
        MovePlayer();
    }

    void MovePlayer()
    {
        if (dashing)
        {
            rb.MovePosition(player.transform.position + new Vector3(Input.GetAxis("Horizontal") * dashSpeed * Time.deltaTime, Input.GetAxis("Vertical") * dashSpeed * Time.deltaTime,0));
        }
        else
        {
            rb.MovePosition(player.transform.position + new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, Input.GetAxis("Vertical") * speed * Time.deltaTime,0));
        }
    }
    
}