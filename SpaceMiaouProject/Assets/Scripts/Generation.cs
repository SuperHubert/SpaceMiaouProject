using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public Case[,] generationGrid;
    public List<Case> generationList;
    public GameObject roomPrefab;

    public int numberOfRooms;
    void Start()
    {
        SetGenerationGrid(numberOfRooms);
        
        Case selectedCase = CreateRoomToGrid(roomPrefab.GetComponent<Case>(),new Vector2Int(numberOfRooms,numberOfRooms), 0);
        
        DebugGenerationGrid();
        
        GetSurroundingCases(selectedCase);

        /*
        for (int i = 1; i < numberOfRooms; i++)
        {
            
        }
        */
    }

    public void SetGenerationGrid(int size)
    {
        generationGrid = new Case[2*(size)+1,2*(size)+1];
    }

    public Case CreateRoomToGrid(Case creator,Vector2Int coords,int creationNumber)
    {
        generationGrid[coords.x, coords.y] = creator.CreateCase(coords,creationNumber);
        generationGrid[coords.x, coords.y].position = coords;

        return generationGrid[coords.x, coords.y];
    }

    public void DebugGenerationGrid()
    {
        string text = "";
        
        for (int i = 0; i < generationGrid.GetLength(0); i++)
        {
            for (int j = 0; j < generationGrid.GetLength(1); j++)
            {
                if (generationGrid[i,j] != null)
                {
                    text += generationGrid[i, j].generationNumber;
                }
                else
                {
                    text += "[empty]";
                }
            }

            text += "\n";
        }
        
        print(text);
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

    public List<Case> GetSurroundingCases(Case targetCase)
    {
        SetSurroundingCases(targetCase);

        List<Case> caseList = new List<Case>();

        if (targetCase.caseAbove != null)
        {
            Case caseAbove = targetCase.caseAbove;
            caseList.Add(caseAbove);
        }
        if (targetCase.caseUnder != null)
        {
            Case caseUnder = targetCase.caseUnder;
            caseList.Add(caseUnder);
        } 
        if (targetCase.caseRight != null)
        {
            Case caseRight = targetCase.caseRight;
            caseList.Add(caseRight);
        }
        if (targetCase.caseLeft != null)
        {
            Case caseLeft = targetCase.caseLeft;
            caseList.Add(caseLeft);
        }
        
        print(caseList);

        return caseList;
    }
}
