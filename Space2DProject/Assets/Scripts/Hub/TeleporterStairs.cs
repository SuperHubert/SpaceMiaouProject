using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterStairs : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("bonkEnter");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("bonkExit");
    }
}
