using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour
{
    public GameObject prefabGameObject;
    
    public Case createdFrom;
    public Case caseAbove;
    public Case caseRight;
    public Case caseUnder;
    public Case caseLeft;

    public GameObject wallTopLeft;
    public GameObject wallTopRight;
    public GameObject wallBotRight;
    public GameObject wallBotLeft;
    public GameObject wallRightTop;
    public GameObject wallRightBot;
    public GameObject wallLeftTop;
    public GameObject wallLeftBot;

    public Vector2Int position = new Vector2Int(0,0);
    public int generationNumber = 0;
    
    public bool IsSurrounded()
    {
        return (caseAbove != null && caseLeft != null && caseRight != null && caseUnder != null);
    }

    public void CloseOutOfBoundsWalls()
    {
        if (caseAbove == null)
        {
            wallTopLeft.SetActive(true);
            wallTopRight.SetActive(true);
        }
        if (caseUnder == null)
        {
            wallBotLeft.SetActive(true);
            wallBotRight.SetActive(true);
        }
        if (caseRight == null)
        {
            wallRightBot.SetActive(true);
            wallRightTop.SetActive(true);
        }
        if (caseLeft == null)
        {
            wallLeftBot.SetActive(true);
            wallLeftTop.SetActive(true);
        }
    }

    public Case CreateCase(Vector2Int creationPosition,int creationNumber)
    {
        GameObject createdCase = Instantiate(prefabGameObject,new Vector3(creationPosition.x*5,creationPosition.y*5,0f),Quaternion.Euler(-90,0,0));
        createdCase.GetComponent<Case>().createdFrom = this;
        createdCase.GetComponent<Case>().generationNumber = creationNumber;
        createdCase.GetComponent<Case>().position = creationPosition;
        
        return createdCase.GetComponent<Case>();
    }
    
}