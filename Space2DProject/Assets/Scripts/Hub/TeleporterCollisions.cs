using UnityEngine;

public class TeleporterCollisions : MonoBehaviour
{
    public Transform player;
    public float high;
    public float low = -2.63f;
    public float offset = 0f;
    private Transform stairSlope;
    
    // Start is called before the first frame update
    void Start()
    {
        //stairSlope = transform.GetChild(0);
        stairSlope = transform;
    }

    // Update is called once per frame
    void Update()
    {
        float y = player.position.y;
        float x = player.position.x;
        if (x > -4.34f)
        {
            if (y < -2.63f) y = -2.63f;
            if (y > -1.677f) y = 1.677f;
            stairSlope.position = new Vector3(-5.3f,y-offset,0);
        }
        else if (x < -5.70f)
        {
            
        }
        
    }
}
