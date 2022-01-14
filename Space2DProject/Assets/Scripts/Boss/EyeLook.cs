using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLook : MonoBehaviour
{
    public Transform player;
    public Transform center;
    public float CenterMinDistance;
    public float CenterMaxDistance;
    public Transform eye1;
    public Transform eye2;
    public Transform eye3;

    public float eye1MinDistance;
    public float eye1MaxDistance;
    public float eye2MinDistance;
    public float eye2MaxDistance;
    public float eye3MinDistance;
    public float eye3MaxDistance;
    public float generalMaxDistance;
    public float distanceToCenter = 0;
    public float distanceToEye = 0;
    
    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(player,Vector3.right);
        
        UpdateDistances();
        
    }
    
    public float eye1x;
    public float eye1xm;
    public float eye1y;
    public float eye1ym;
    public float eye1z;
    public float eye1zm;
    public float eye2x;
    public float eye2xm;
    public float eye2y;
    public float eye2ym;
    public float eye2z;
    public float eye2zm;
    public float eye3x;
    public float eye3xm;
    public float eye3y;
    public float eye3ym;
    public float eye3z;
    public float eye3zm;
    private void UpdateDistances()
    {
        
        distanceToEye = Vector3.Distance(player.position, transform.position);
        if(distanceToEye == 0) return;
        center.localPosition = Mathf.Lerp(CenterMaxDistance, CenterMinDistance, distanceToEye / generalMaxDistance) * Vector3.forward;
        
        distanceToCenter = Vector3.Distance(player.position, center.position);
        if(generalMaxDistance == 0) return;
        
        eye1.localPosition = Mathf.Lerp(eye1MaxDistance, eye1MinDistance, distanceToCenter / generalMaxDistance) * Vector3.forward;
        eye2.localPosition = Mathf.Lerp(eye2MaxDistance, eye2MinDistance, distanceToCenter / generalMaxDistance) * Vector3.forward;
        eye3.localPosition = Mathf.Lerp(eye3MaxDistance, eye3MinDistance, distanceToCenter / generalMaxDistance) * Vector3.forward;

        eye1.localScale = Mathf.Lerp(eye1x, eye1xm, distanceToCenter / generalMaxDistance) * Vector3.right +
                          Mathf.Lerp(eye1y, eye1ym, distanceToCenter / generalMaxDistance) * Vector3.up +
                          Mathf.Lerp(eye1z, eye1zm, distanceToCenter / generalMaxDistance) * Vector3.forward;

        eye2.localScale = Mathf.Lerp(eye2x, eye2xm, distanceToCenter / generalMaxDistance) * Vector3.right +
                          Mathf.Lerp(eye2y, eye2ym, distanceToCenter / generalMaxDistance) * Vector3.up +
                          Mathf.Lerp(eye2z, eye2zm, distanceToCenter / generalMaxDistance) * Vector3.forward;
        
        eye3.localScale = Mathf.Lerp(eye3x, eye3xm, distanceToCenter / generalMaxDistance) * Vector3.right +
                          Mathf.Lerp(eye3y, eye3ym, distanceToCenter / generalMaxDistance) * Vector3.up +
                          Mathf.Lerp(eye3z, eye3zm, distanceToCenter / generalMaxDistance) * Vector3.forward;


    }
}
