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
    
    private List<GameObject>[,] prefabGrid = new List<GameObject>[3,15];

    public void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                prefabGrid[i, j] = new List<GameObject>();
            }
        }
    }

    public void FillAllPools()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                RefillPool(i,j);
            }
        }
    }

    private void RefillPool(int b, int n)
    {
        switch (b)
        {
            case 0:
                switch (n)
                {
                    case 0:
                        RefillList(prefabGrid[0, 0],b1Ver4);
                        break;
                
                    case 1:
                        RefillList(prefabGrid[0, 1],b1Ver31);
                        break;
                
                    case 2:
                        RefillList(prefabGrid[0, 2] ,b1Ver32);
                        break;
                
                    case 3:
                        RefillList(prefabGrid[0, 3] ,b1Ver33);
                        break;
                
                    case 4:
                        RefillList(prefabGrid[0, 4] ,b1Ver34);
                        break;
                
                    case 5:
                        RefillList(prefabGrid[0, 5] ,b1Ver21);
                        break;
                
                    case 6:
                        RefillList(prefabGrid[0, 6] ,b1Ver22);
                        break;
                
                    case 7:
                        RefillList(prefabGrid[0, 7] ,b1Ver23);
                        break;
                
                    case 8:
                        RefillList(prefabGrid[0, 8] ,b1Ver24);
                        break;
                
                    case 9:
                        RefillList(prefabGrid[0, 9] ,b1Ver25);
                        break;
                
                    case 10:
                        RefillList(prefabGrid[0, 10],b1Ver26);
                        break;
                
                    case 11:
                        RefillList(prefabGrid[0, 11],b1Ver11);
                        break;
                
                    case 12:
                        RefillList(prefabGrid[0, 12],b1Ver12);
                        break;
                
                    case 13:
                        RefillList(prefabGrid[0, 13],b1Ver13);
                        break;
                
                    case 14:
                        RefillList(prefabGrid[0, 14],b1Ver14);
                        break;
                }

                break;
            case 1:
                switch (n)
                {
                    case 0:
                        RefillList(prefabGrid[1, 0], b2Ver4);
                        break;
                
                    case 1:
                        RefillList(prefabGrid[1, 1] ,b2Ver31);
                        break;
                
                    case 2:
                        RefillList(prefabGrid[1, 2] ,b2Ver32);
                        break;
                
                    case 3:
                        RefillList(prefabGrid[1, 3] ,b2Ver33);
                        break;
                
                    case 4:
                        RefillList(prefabGrid[1, 4] ,b2Ver34);
                        break;
                
                    case 5:
                        RefillList(prefabGrid[1, 5] ,b2Ver21);
                        break;
                
                    case 6:
                        RefillList(prefabGrid[1, 6] ,b2Ver22);
                        break;
                
                    case 7:
                        RefillList(prefabGrid[1, 7] ,b2Ver23);
                        break;
                
                    case 8:
                        RefillList(prefabGrid[1, 8] ,b2Ver24);
                        break;
                
                    case 9:
                        RefillList(prefabGrid[1, 9] ,b2Ver25);
                        break;
                
                    case 10:
                        RefillList(prefabGrid[1, 10],b2Ver26);
                        break;
                
                    case 11:
                        RefillList(prefabGrid[1, 11],b2Ver11);
                        break;
                
                    case 12:
                        RefillList(prefabGrid[1, 12],b2Ver12);
                        break;
                
                    case 13:
                        RefillList(prefabGrid[1, 13],b2Ver13);
                        break;
                
                    case 14:
                        RefillList(prefabGrid[1, 14],b2Ver14);
                        break;
                }

                break;
            case 2:
                switch (n)
                {
                    case 0:
                         RefillList(prefabGrid[2, 0] ,b3Ver4);
                        break;
                
                    case 1:
                        RefillList(prefabGrid[2, 1] ,b3Ver31);
                        break;
                
                    case 2:
                        RefillList(prefabGrid[2, 2] ,b3Ver32);
                        break;
                
                    case 3:
                        RefillList(prefabGrid[2, 3] ,b3Ver33);
                        break;
                
                    case 4:
                        RefillList(prefabGrid[2, 4] ,b3Ver34);
                        break;
                
                    case 5:
                        RefillList(prefabGrid[2, 5] ,b3Ver21);
                        break;
                
                    case 6:
                        RefillList(prefabGrid[2, 6] ,b3Ver22);
                        break;
                
                    case 7:
                        RefillList(prefabGrid[2, 7] ,b3Ver23);
                        break;
                
                    case 8:
                        RefillList(prefabGrid[2, 8] ,b3Ver24);
                        break;
                
                    case 9:
                        RefillList(prefabGrid[2, 9] ,b3Ver25);
                        break;
                
                    case 10:
                        RefillList(prefabGrid[2, 10],b3Ver26);
                        break;
                
                    case 11:
                        RefillList(prefabGrid[2, 11],b3Ver11);
                        break;
                
                    case 12:
                        RefillList(prefabGrid[2, 12],b3Ver12);
                        break;
                
                    case 13:
                        RefillList(prefabGrid[2, 13],b3Ver13);
                        break;
                
                    case 14:
                        RefillList(prefabGrid[2, 14],b3Ver14);
                        break;
                }

                break;
        }
    }

    private void RefillList(List<GameObject> target, List<GameObject> origin)
    {
        target.Clear();
        foreach (GameObject item in origin)
        {
            target.Add(item);
        }
    }

    public GameObject GetRoom(bool isTop, bool isBot, bool isLeft, bool isRight)
    {
        int currentRegion = LevelManager.Instance.GetBiome();

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
            switch (spriteNumber)
            {
                
                case 2 * 3 * 5 * 7: //210
                    return PickRoom(currentRegion,0);
                
                case 2 * 5 * 7: //70
                    return PickRoom(currentRegion,1);
                
                case 3 * 5 * 7: //105
                    return  PickRoom(currentRegion,2);
                
                case 2 * 3 * 7: //42
                    return  PickRoom(currentRegion,3);
                
                case 2 * 3 * 5: //30
                    return  PickRoom(currentRegion,4);

                case 2 * 3: //6
                    return  PickRoom(currentRegion,5);

                case 5 * 7: //35
                    return  PickRoom(currentRegion,6);

                case 2 * 7: //14
                    return  PickRoom(currentRegion,7);

                case 2 * 5: //10
                    return  PickRoom(currentRegion,8);

                case 3 * 5: //15
                    return  PickRoom(currentRegion,9);

                case 3 * 7: //21
                     return PickRoom(currentRegion,10);

                case 2:
                     return PickRoom(currentRegion,11);

                case 7:
                     return PickRoom(currentRegion,12);

                case 3:
                     return PickRoom(currentRegion,13);

                case 5:
                     return PickRoom(currentRegion,14);

                default:
                    return PickRoom(0,0);;
                    
            }
        }
    }

    private void CheckRefill(int b, int n)
    {
        Debug.Log("PrefabGrid["+b+","+n+"].Count="+prefabGrid[b, n].Count);
        if (prefabGrid[b, n].Count == 0)
        {
            RefillPool(b, n);
        }
        
    }

    private GameObject PickRoom(int region, int n)
    {
        CheckRefill(region,n);

        int a = prefabGrid[region, n].Count - 1;
        
        Debug.Log("a = "+a);
        foreach (var VARIABLE in prefabGrid[region,n])
        {
            Debug.Log(VARIABLE);
            
        }
        
        GameObject target = prefabGrid[region, 0][Random.Range(0,a)];
        
        prefabGrid[region, 0].Remove(target);

        return target;
    }
    


    
}
