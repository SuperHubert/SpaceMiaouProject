using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
   [SerializeField] private Transform cameraTransform;
   [SerializeField] private Transform target;

   [SerializeField] private Vector3 offset = new Vector3(0,0,-10);

   [SerializeField] private float smoothSpeed = 0.1f;

   private void FixedUpdate()
   {
      Vector3 desiredPosition = target.position + offset;
      Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, smoothSpeed);
      cameraTransform.position = smoothedPosition;
   }
}
