using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAssigner : MonoBehaviour
{
    public Sprite allDirections;
    public Sprite threeDirections1;
    public Sprite threeDirections2;
    public Sprite threeDirections3;
    public Sprite threeDirections4;
    public Sprite twoDirections1;
    public Sprite twoDirections2;
    public Sprite twoDirections3;
    public Sprite twoDirections4;
    public Sprite twoDirections5;
    public Sprite twoDirections6;
    public Sprite oneDirection1;
    public Sprite oneDirection2;
    public Sprite oneDirection3;
    public Sprite oneDirection4;
    
    public Sprite GetSprite(bool isTop, bool isBot, bool isLeft, bool isRight)
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
                return allDirections;
            case 2*5*7:
                return threeDirections1;
            case 5*3*7:
                return threeDirections2;
            case 2*7*3:
                return threeDirections3;
            case 2*3*5:
                return threeDirections4;
            case 2*3:
                return twoDirections1;
            case 5*7:
                return twoDirections2;
            case 2*7:
                return twoDirections3;
            case 2*5:
                return twoDirections4;
            case 3*5:
                return twoDirections5;
            case 3*7:
                return twoDirections6;
            case 2:
                return oneDirection1;
            case 7:
                return oneDirection2;
            case 3:
                return oneDirection3;
            case 5:
                return oneDirection4;
            default:
                return allDirections;
        }
    }
    
    


    
}
