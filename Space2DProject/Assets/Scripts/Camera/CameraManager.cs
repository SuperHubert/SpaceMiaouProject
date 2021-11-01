using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
   public Transform target;

   public Vector3 offset;

   public float smoothSpeed = 0.1f;

   private void FixedUpdate()
   {
      Vector3 desiredPosition = target.position + offset;
      Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
      transform.position = smoothedPosition;
   }
}