using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractible
{
    
    public void OnInteraction()
    {
        Debug.Log("bonk");
    }
}
