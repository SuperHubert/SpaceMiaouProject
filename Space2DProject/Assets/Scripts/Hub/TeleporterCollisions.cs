using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterCollisions : MonoBehaviour
{
    public Transform player;
    public float high;
    public float low = -2.63f;
    private Transform stairSlope;
    
    // Start is called before the first frame update
    void Start()
    {
        stairSlope = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        float y = player.position.y;
        float x = player.position.x;
        if (x > -4.36f)
        {
            if (y > -2.63 && y < -1.677)
            {
                stairSlope.localPosition = new Vector3(0,y + low,0);
            }
        }
        else if (x < -5.70f)
        {
            
        }
        
    }
}
