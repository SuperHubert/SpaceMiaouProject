using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TextureAssigner : MonoBehaviour
{
    [SerializeField] private bool useOldMethod = true;
    [Space(10)]
    [Header("Old Method Settings")]
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
    [Space(10)]
    
    [Header("Biome 1")]
    [SerializeField] private List<GameObject> b1Ver4;
    [SerializeField] private List<GameObject> b1Ver31;
    [SerializeField] private List<GameObject> b1Ver32;
    [SerializeField] private List<GameObject> b1Ver33;
    [SerializeField] private List<GameObject> b1Ver34;
    [SerializeField] private List<GameObject> b1Ver21;
    [SerializeField] private List<GameObject> b1Ver22;
    [SerializeField] private List<GameObject> b1Ver23;
    [SerializeField] private List<GameObject> b1Ver24;
    [SerializeField] private List<GameObject> b1Ver25;
    [SerializeField] private List<GameObject> b1Ver26;
    [SerializeField] private List<GameObject> b1Ver11;
    [SerializeField] private List<GameObject> b1Ver12;
    [SerializeField] private List<GameObject> b1Ver13;
    [SerializeField] private List<GameObject> b1Ver14;
    [Space(10)]
    [Header("Biome 2")]
    [SerializeField] private List<GameObject> b2Ver4;
    [SerializeField] private List<GameObject> b2Ver31;
    [SerializeField] private List<GameObject> b2Ver32;
    [SerializeField] private List<GameObject> b2Ver33;
    [SerializeField] private List<GameObject> b2Ver34;
    [SerializeField] private List<GameObject> b2Ver21;
    [SerializeField] private List<GameObject> b2Ver22;
    [SerializeField] private List<GameObject> b2Ver23;
    [SerializeField] private List<GameObject> b2Ver24;
    [SerializeField] private List<GameObject> b2Ver25;
    [SerializeField] private List<GameObject> b2Ver26;
    [SerializeField] private List<GameObject> b2Ver11;
    [SerializeField] private List<GameObject> b2Ver12;
    [SerializeField] private List<GameObject> b2Ver13;
    [SerializeField] private List<GameObject> b2Ver14;
    [Space(10)]
    [Header("Biome 3")]
    [SerializeField] private List<GameObject> b3Ver4;
    [SerializeField] private List<GameObject> b3Ver31;
    [SerializeField] private List<GameObject> b3Ver32;
    [SerializeField] private List<GameObject> b3Ver33;
    [SerializeField] private List<GameObject> b3Ver34;
    [SerializeField] private List<GameObject> b3Ver21;
    [SerializeField] private List<GameObject> b3Ver22;
    [SerializeField] private List<GameObject> b3Ver23;
    [SerializeField] private List<GameObject> b3Ver24;
    [SerializeField] private List<GameObject> b3Ver25;
    [SerializeField] private List<GameObject> b3Ver26;
    [SerializeField] private List<GameObject> b3Ver11;
    [SerializeField] private List<GameObject> b3Ver12;
    [SerializeField] private List<GameObject> b3Ver13;
    [SerializeField] private List<GameObject> b3Ver14;
    
    //pools
    private List<GameObject> b1V4;
    private List<GameObject> b1V31;
    private List<GameObject> b1V32;
    private List<GameObject> b1V33;
    private List<GameObject> b1V34;
    private List<GameObject> b1V21;
    private List<GameObject> b1V22;
    private List<GameObject> b1V23;
    private List<GameObject> b1V24;
    private List<GameObject> b1V25;
    private List<GameObject> b1V26;
    private List<GameObject> b1V11;
    private List<GameObject> b1V12;
    private List<GameObject> b1V13;
    private List<GameObject> b1V14;
    private List<GameObject> b2V4;
    private List<GameObject> b2V31;
    private List<GameObject> b2V32;
    private List<GameObject> b2V33;
    private List<GameObject> b2V34;
    private List<GameObject> b2V21;
    private List<GameObject> b2V22;
    private List<GameObject> b2V23;
    private List<GameObject> b2V24;
    private List<GameObject> b2V25;
    private List<GameObject> b2V26;
    private List<GameObject> b2V11;
    private List<GameObject> b2V12;
    private List<GameObject> b2V13;
    private List<GameObject> b2V14;
    private List<GameObject> b3V4;
    private List<GameObject> b3V31;
    private List<GameObject> b3V32; 
    private List<GameObject> b3V33;
    private List<GameObject> b3V34;
    private List<GameObject> b3V21;
    private List<GameObject> b3V22;
    private List<GameObject> b3V23;
    private List<GameObject> b3V24;
    private List<GameObject> b3V25;
    private List<GameObject> b3V26;
    private List<GameObject> b3V11;
    private List<GameObject> b3V12;
    private List<GameObject> b3V13;
    private List<GameObject> b3V14;

    private void Start()
    {
        
    }

    public void RefillPool(int b, int n)
    {
        if (b == 1)
        {
            switch (n)
            {
                case 4:
                    b1V4 = b1Ver4;
                    break;
                
                case 31:
                    b1V31 = b1Ver31;
                    break;
                
                case 32:
                    b1V32 = b1Ver32;
                    break;
                
                case 33:
                    b1V33 = b1Ver33;
                    break;
                
                case 34:
                    b1V34 = b1Ver34;
                    break;
                
                case 11:
                    b1V11 = b1Ver11;
                    break;
                
                case 12:
                    b1V12 = b1Ver11;
                    break;
                
                case 13:
                    b1V13 = b1Ver13;
                    break;
                
                case 14:
                    b1V14 = b1Ver14;
                    break;
                
                case 21:
                    b1V4 = b1Ver4;
                    break;
                
                case 22:
                    b1V4 = b1Ver4;
                    break;
                
                case 23:
                    b1V4 = b1Ver4;
                    break;
                
                case 24:
                    b1V4 = b1Ver4;
                    break;
                
                case 25:
                    b1V4 = b1Ver4;
                    break;
                
                case 26:
                    b1V4 = b1Ver4;
                    break;
                
                
            }
        }
        else if (b == 2)
        {
        }
        else if (b == 3)
        {
            
        }
    }

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

        if (useOldMethod)
        {
            return spriteNumber switch
            {
                2 * 3 * 5 * 7 => allDirections[Random.Range(0, allDirections.Count)],
                2 * 5 * 7 => threeDirections1[Random.Range(0, threeDirections1.Count)],
                5 * 3 * 7 => threeDirections2[Random.Range(0, threeDirections2.Count)],
                2 * 7 * 3 => threeDirections3[Random.Range(0, threeDirections3.Count)],
                2 * 3 * 5 => threeDirections4[Random.Range(0, threeDirections4.Count)],
                2 * 3 => twoDirections1[Random.Range(0, twoDirections1.Count)],
                5 * 7 => twoDirections2[Random.Range(0, twoDirections2.Count)],
                2 * 7 => twoDirections3[Random.Range(0, twoDirections3.Count)],
                2 * 5 => twoDirections4[Random.Range(0, twoDirections4.Count)],
                3 * 5 => twoDirections5[Random.Range(0, twoDirections5.Count)],
                3 * 7 => twoDirections6[Random.Range(0, twoDirections6.Count)],
                2 => oneDirection1[Random.Range(0, oneDirection1.Count)],
                7 => oneDirection2[Random.Range(0, oneDirection2.Count)],
                3 => oneDirection3[Random.Range(0, oneDirection3.Count)],
                5 => oneDirection4[Random.Range(0, oneDirection4.Count)],
                _ => allDirections[0]
            };
        }
        else
        {
            return allDirections[0];
        }
        
    }
    
    


    
}
