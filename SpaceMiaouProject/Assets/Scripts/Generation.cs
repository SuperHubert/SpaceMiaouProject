using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public Case[,] generationGrid;
    public Case[] generationList;
    public GameObject room;

    public int numberOfRooms;
    void Start()
    {
        Case selectedCase;
        
        generationGrid = new Case[2*(numberOfRooms-1)+1,2*(numberOfRooms-1)+1];
        generationList = new Case[numberOfRooms];
        selectedCase = room.GetComponent<Case>().CreateCase(new Vector2Int(numberOfRooms,numberOfRooms),0);

        selectedCase.CreateCase(new Vector2Int(numberOfRooms,numberOfRooms+1),1);

        GetSurroundingCases(selectedCase);

        for (int i = 1; i < numberOfRooms; i++)
        {
            
        }
    }

    public void SetSurroundingCases(Case targetCase)
    {
        if (generationGrid[targetCase.position.x, targetCase.position.y + 1] != null)
        {
            targetCase.caseAbove = generationGrid[targetCase.position.x, targetCase.position.y + 1];
        }
        if (generationGrid[targetCase.position.x, targetCase.position.y - 1] != null)
        {
            targetCase.caseUnder = generationGrid[targetCase.position.x, targetCase.position.y - 1];
        } 
        if (generationGrid[targetCase.position.x + 1, targetCase.position.y] != null)
        {
            targetCase.caseRight = generationGrid[targetCase.position.x + 1, targetCase.position.y];
        }
        if (generationGrid[targetCase.position.x - 1, targetCase.position.y] != null)
        {
            targetCase.caseLeft = generationGrid[targetCase.position.x - 1, targetCase.position.y];
        }
    }

    public Case[] GetSurroundingCases(Case targetCase)
    {
        SetSurroundingCases(targetCase);
        
        int listSize = 0;
        Case caseAbove;
        Case caseUnder;
        Case caseRight;
        Case caseLeft;
        
        if (targetCase.caseAbove != null)
        {
            caseAbove = targetCase.caseAbove;
            listSize++;
        }
        if (targetCase.caseUnder != null)
        {
            caseUnder = targetCase.caseUnder;
            listSize++;
        } 
        if (targetCase.caseRight != null)
        {
            caseRight = targetCase.caseRight;
            listSize++;
        }
        if (targetCase.caseLeft != null)
        {
            caseLeft = targetCase.caseLeft;
            listSize++;
        }

        Case[] caseList = new Case[listSize];
        
        print(caseList);

        return caseList;
    }
}
