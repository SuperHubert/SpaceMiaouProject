using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTempBehaviour : EnemyBehaviour
{
    private void Update()
    {
        if(currentState != State.Awake) return;
    }
}