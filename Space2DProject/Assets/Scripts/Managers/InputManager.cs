using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool canInput = true;
    public GameObject playerObj;

    private SprayAttack sprayAttack;
    private Combat combat;

    void Start()
    {
        sprayAttack = playerObj.GetComponent<SprayAttack>();
        combat = playerObj.GetComponent<Combat>();
    }
    
    void Update()
    {
        if(!canInput) return;
        
        sprayAttack.sprayAttackAxis = Input.GetAxisRaw("SprayAttack");

        combat.rightAttack = Input.GetButtonDown("RightAttack");
        combat.leftAttack = Input.GetButtonDown("LeftAttack");
        combat.uptAttack = Input.GetButtonDown("UpAttack");
        combat.downAttack = Input.GetButtonDown("DownAttack");
        combat.specialAttack = Input.GetButtonDown("SpecialAttack");
    }
}
