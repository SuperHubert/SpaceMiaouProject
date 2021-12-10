using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool canInput = true;
    public bool canInputMirror;
    public GameObject playerObj;

    private SprayAttack sprayAttack;
    private Combat combat;
    private DisplayInteracion displayInteraction;
    private PlayerMovement playerMovement;

    void Start()
    {
        sprayAttack = playerObj.GetComponent<SprayAttack>();
        combat = playerObj.GetComponent<Combat>();
        displayInteraction = playerObj.GetComponent<DisplayInteracion>();
        playerMovement = playerObj.GetComponent<PlayerMovement>();
    }
    
    void Update()
    {
        canInputMirror = canInput;
        if(!canInput) return;
        
        sprayAttack.sprayAttackAxis = Input.GetAxisRaw("SprayAttack");

        combat.rightAttack = Input.GetButtonDown("RightAttack");
        combat.leftAttack = Input.GetButtonDown("LeftAttack");
        combat.uptAttack = Input.GetButtonDown("UpAttack");
        combat.downAttack = Input.GetButtonDown("DownAttack");
        combat.specialAttack = Input.GetButtonDown("SpecialAttack");

        displayInteraction.interact = Input.GetButtonDown("Fire1");
        
        playerMovement.horizontalAxis = Input.GetAxisRaw("Horizontal");
        playerMovement.verticalAxis = Input.GetAxisRaw("Vertical");
        //playerMovement.dash = Input.GetButtonDown("Dash");
        playerMovement.dash = Input.GetMouseButtonDown(1);
    }
}
