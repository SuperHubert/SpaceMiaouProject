using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Case : MonoBehaviour
{
    public GameObject prefabGameObject;
    
    public GameObject createdFrom;
    public Case caseAbove;
    public Case caseRight;
    public Case caseUnder;
    public Case caseLeft;

    public bool isEmpty = false;
    
    public Vector2Int position = new Vector2Int(0,0);
    public int generationNumber = 0;
    
    public bool IsSurrounded()
    {
        return (caseAbove != null && caseLeft != null && caseRight != null && caseUnder != null);
    }

    public void CloseOutOfBoundsWalls()
    {
        if (isEmpty)
        {
            Destroy(this.gameObject);
        }
    }

    public Case CreateCase(Vector2Int creationPosition,int offset,int creationNumber)
    {
        GameObject createdCase = Instantiate(prefabGameObject,new Vector3((creationPosition.x-offset)*50,(creationPosition.y-offset)*50),quaternion.identity);
        createdCase.GetComponent<Case>().createdFrom = gameObject;
        createdCase.GetComponent<Case>().generationNumber = creationNumber;
        createdCase.GetComponent<Case>().position = creationPosition;
        
        return createdCase.GetComponent<Case>();
    }

}