using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRotationOnStart : MonoBehaviour
{
    public Vector3 rot;
    void Start()
    {
       transform.localRotation = Quaternion.Euler(rot); 
    }
    
    
}
