using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La classe qui gere les mouvement du joueur.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    //Player RigidBody
    private Rigidbody2D rb;
    [SerializeField] private Fall fall;

    //normal movement
    public float speed = 10f;
    public Vector3 inputMovement;
    [SerializeField] private float deadZone = 0.3f;
    
    public Vector3 lastDirection;

    //dash related
    public float dashSpeed = 40f;
    public bool dashing = false;
    private float dashInternalCd;
    public float dashInternalCdMax = 0.1f;
    private float dashCd;
    public float dashCdMax = 1f;

    //Animations
    [SerializeField] private Animator animPlayer;
    public int playerDirection = 0;
    
    //Inputs
    public float horizontalAxis;
    public float verticalAxis;
    public bool dash;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashInternalCd = 0;
        dashCd = 0;
    }

    private void Update()
    {
        inputMovement.x = horizontalAxis;
        inputMovement.y = verticalAxis;
        
        if (Mathf.Abs(horizontalAxis) > 0.3f || Mathf.Abs(verticalAxis) > 0.3f)
        {
            lastDirection = inputMovement;
        }
        inputMovement.Normalize();
        
        if (dash)
        {
            if (dashCd <= 0)
            {
                dashCd = dashCdMax;
                dashInternalCd = 0;
                dashing = true;
            }
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        AnimationPlayer();
        
        //Dash
        dashCd -= Time.fixedDeltaTime;
        
        if (dashing)
        {
            if (dashInternalCd>dashInternalCdMax)
            {
                rb.velocity = Vector2.zero;
                dashing = false;
                StartCoroutine(fall.TeleportFollower());
            }
            else
            {
                dashInternalCd += Time.fixedDeltaTime;
                rb.velocity = inputMovement * dashSpeed;
            }
        }
        else
        {
            
        }
        
    }

    void MovePlayer()
    {
        rb.velocity = Vector2.zero;
        if(!InputManager.canInput) return;
        if (Mathf.Abs(inputMovement.x) > deadZone || Mathf.Abs(inputMovement.y) > deadZone)
        {
            if (GetComponent<Combat>().isAttacking == false && GetComponent<SprayAttack>().isSpraying == false)
            {
                animPlayer.SetBool("IsWalking",true); 
                rb.velocity = inputMovement * speed;  
            }
        }
        else
        {
            animPlayer.SetBool("IsWalking",false);
        }
    }

    void AnimationPlayer()
    {
        if (inputMovement.x > deadZone)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (inputMovement.x < -deadZone)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (inputMovement.y < -deadZone && Mathf.Abs(inputMovement.x) < deadZone)
        {
            if (playerDirection != 0)
            {
                playerDirection = 0;
                animPlayer.SetTrigger("GoingDown");
            }
        }
        else if (inputMovement.y > deadZone && Mathf.Abs(inputMovement.x) < deadZone)
        {
            if (playerDirection != 1)
            {
                playerDirection = 1;
                animPlayer.SetTrigger("GoingUp");
            }
        }
        else if (Mathf.Abs(inputMovement.y) < deadZone && Mathf.Abs(inputMovement.x) > deadZone)
        {
            if (playerDirection != 2)
            {
                playerDirection = 2;
                animPlayer.SetTrigger("GoingSide");
            }
        }
        else if (inputMovement.y < -deadZone && Mathf.Abs(inputMovement.x) > deadZone)
        {
            if (playerDirection != 3)
            {
                playerDirection = 3;
                animPlayer.SetTrigger("GoingDownSide");
            }
        }
        else if (inputMovement.y > deadZone && Mathf.Abs(inputMovement.x) > deadZone)
        {
            if (playerDirection != 4)
            {
                playerDirection = 4;
                animPlayer.SetTrigger("GoingUpSide");
            }
        }
    }
}