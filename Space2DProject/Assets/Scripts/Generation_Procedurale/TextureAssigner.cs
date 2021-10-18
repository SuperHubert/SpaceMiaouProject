using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAssigner : MonoBehaviour
{
    public List<GameObject> allDirections;
    public List<GameObject> threeDirections1;
    public List<GameObject> threeDirections2;
    public List<GameObject> threeDirections3;
    public List<GameObject> threeDirections4;
    public List<GameObject> twoDirections1;
    public List<GameObject> twoDirections2;
    public List<GameObject> twoDirections3;
    public List<GameObject> twoDirections4;
    public List<GameObject> twoDirections5;
    public List<GameObject> twoDirections6;
    public List<GameObject> oneDirection1;
    public List<GameObject> oneDirection2;
    public List<GameObject> oneDirection3;
    public List<GameObject> oneDirection4;
    
    public GameObject GetRoom(bool isTop, bool isBot, bool isLeft, bool isRight)
    {
        int a = 1;
        int b = 1;
        int c = 1;
        int d = 1;
        
        if (isTop)
        {
            a = 2;
        }
        if (isBot)
        {
            b = 3;
        }
        if (isLeft)
        {
            c = 5;
        }
        if (isRight)
        {
            d = 7;
        }

        int spriteNumber = a * b * c * d;

        switch (spriteNumber)
        {
            
            case 2*3*5*7:
                return allDirections[Random.Range(0,allDirections.Count)];
            case 2*5*7:
                return threeDirections1[Random.Range(0,threeDirections1.Count)];
            case 5*3*7:
                return threeDirections2[Random.Range(0,threeDirections2.Count)];
            case 2*7*3:
                return threeDirections3[Random.Range(0,threeDirections3.Count)];
            case 2*3*5:
                return threeDirections4[Random.Range(0,threeDirections4.Count)];
            case 2*3:
                return twoDirections1[Random.Range(0,twoDirections1.Count)];
            case 5*7:
                return twoDirections2[Random.Range(0,twoDirections2.Count)];
            case 2*7:
                return twoDirections3[Random.Range(0,twoDirections3.Count)];
            case 2*5:
                return twoDirections4[Random.Range(0,twoDirections4.Count)];
            case 3*5:
                return twoDirections5[Random.Range(0,twoDirections5.Count)];
            case 3*7:
                return twoDirections6[Random.Range(0,twoDirections6.Count)];
            case 2:
                return oneDirection1[Random.Range(0,oneDirection1.Count)];
            case 7:
                return oneDirection2[Random.Range(0,oneDirection2.Count)];
            case 3:
                return oneDirection3[Random.Range(0,oneDirection3.Count)];
            case 5:
                return oneDirection4[Random.Range(0,oneDirection4.Count)];
            default:
                return allDirections[0];
        }
    }
    
    


    
}
