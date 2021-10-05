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
        selectedCase.name = 0.ToString();
        
        for (int i = 1; i < numberOfRooms; i++)
        {
            Vector2Int nextPos = GetNextPosition(selectedCase);

            selectedCase = CreateRoomToGrid(roomPrefab.GetComponent<Case>(),nextPos, i);
            selectedCase.name = selectedCase.generationNumber.ToString();
            
            GetSurroundingCases(selectedCase);
        }
        
        
        foreach (Case room in generationList)
        {
            GetSurroundingCases(room);
            room.CloseOutOfBoundsWalls();
        }
        
    }

    void SetGenerationGrid(int size)
    {
        generationGrid = new Case[2*(size)+1,2*(size)+1];
    }

    Case CreateRoomToGrid(Case creator,Vector2Int coords,int creationNumber)
    {
        generationGrid[coords.x, coords.y] = creator.CreateCase(coords,numberOfRooms,creationNumber);
        generationGrid[coords.x, coords.y].position = coords;
        generationList.Add(generationGrid[coords.x, coords.y]);
        
        return generationGrid[coords.x, coords.y];
    }
    
    Vector2Int GetNextPosition(Case currentRoom)
    {
        List<int> surroundingRoomsList = GetSurroundingCases(currentRoom);
        
        if (surroundingRoomsList.Count == 4)
        {
            currentRoom = GetCaseFromNumber(currentRoom.generationNumber - 1);

            return GetNextPosition(currentRoom);
        }
        else
        {
            List<int> availablePositionsList = GetAvailablePositionsFromUnavailablePositions(surroundingRoomsList);

            int selectedPosition = availablePositionsList[Random.Range(0, availablePositionsList.Count)];

            return ConvertIntToPosition(selectedPosition, currentRoom);
        }
    }
    
    List<int> GetSurroundingCases(Case targetCase)
    {
        SetAllSurroundingCases(targetCase);
        List<int> caseList = new List<int>();
        
        if (targetCase.caseAbove != null)
        {
            caseList.Add(1);
        }

        if (targetCase.caseUnder != null)
        {
            caseList.Add(3);
        }
        
        if (targetCase.caseRight != null)
        {
            caseList.Add(2);
        }

        if (targetCase.caseLeft != null)
        {
            caseList.Add(4);
        }
        
        return caseList;
    }
    
    void SetAllSurroundingCases(Case targetCase)
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
    
    Case GetCaseFromNumber(int number)
    {
        foreach (Case room in generationList)
        {
            if (room.generationNumber == number)
            {
                return room;
            }
        }
        
        return null;
    }
    
    List<int> GetAvailablePositionsFromUnavailablePositions(List<int> unavailablePositionsList)
    {
        List<int> availablePositionsList = new List<int>() {1,2,3,4};
        foreach (int position in unavailablePositionsList)
        {
            if (availablePositionsList.Contains(position))
            {
                availablePositionsList.Remove(position);
            }
        }

        return availablePositionsList;
    }
    
    Vector2Int ConvertIntToPosition(int position,Case room)
    {
        switch (position)
        {
            case 1:
                return new Vector2Int(room.position.x, room.position.y + 1);
            case 2:
                return new Vector2Int(room.position.x + 1, room.position.y);
            case 3:
                return new Vector2Int(room.position.x, room.position.y - 1);
            case 4:
                return new Vector2Int(room.position.x - 1, room.position.y);
            default:
                return Vector2Int.zero;
        }
    }

    
    Vector2Int ConvertPositionTo00(Vector2Int position)
    {
        return new Vector2Int(position.x - numberOfRooms/5, position.y - numberOfRooms/5);
    }
    
}
