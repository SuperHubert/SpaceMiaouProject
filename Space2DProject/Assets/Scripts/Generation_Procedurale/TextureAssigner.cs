using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
