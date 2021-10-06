using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerationV2 : MonoBehaviour
{
    Case[,] generationGrid;
    public List<Case> generationList;
    public GameObject roomPrefab;

    public int numberOfRooms;

    public Transform parent;
    public string parentName;

    private int checkpointNumber;
    private bool checkNextInsteadOfPrevious = false;
    void Start()
    {
        GenerateRooms(numberOfRooms,parent);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            GameObject newParent = new GameObject(parentName);
            GenerateRooms(numberOfRooms,newParent.transform);
        }
    }

    void GenerateRooms(int number,Transform parentObj)
    {
        SetGenerationGrid(number);
        
        Case selectedCase = CreateRoom(roomPrefab.GetComponent<Case>(),new Vector2Int(number,number), 0,"0",parentObj);

        for (int i = 1; i < number; i++)
        {
            checkpointNumber = selectedCase.generationNumber;
            checkNextInsteadOfPrevious = false;
            Vector2Int nextPos = GetNextPosition(selectedCase);

            if (Random.Range(0, 2) == 1)
            {
                Case newRoom = CreateRoom(roomPrefab.GetComponent<Case>(),nextPos, i, i.ToString(),parentObj);
                UpdateAllSurroundingCases(newRoom);
            }
            else
            {
                selectedCase = CreateRoom(roomPrefab.GetComponent<Case>(),nextPos, i, i.ToString(),parentObj);
                UpdateAllSurroundingCases(selectedCase);
            }
            
        }

        UpdateRoomAppearance();
        
        CleanUpGrid();
    }

    void SetGenerationGrid(int size)
    {
        generationGrid = new Case[2*(size)+1,2*(size)+1];
    }
    
    Case CreateRoom(Case creator,Vector2Int coords,int creationNumber, string caseName ,Transform caseParent)
    {
        Case createdRoom = creator.CreateCase(coords,numberOfRooms,creationNumber);
        createdRoom.position = coords;
        createdRoom.name = caseName;
        createdRoom.transform.parent = caseParent;
        
        generationGrid[coords.x, coords.y] = createdRoom;
        generationList.Add(generationGrid[coords.x, coords.y]);
        
        return generationGrid[coords.x, coords.y];
    }

    Vector2Int GetNextPosition(Case currentRoom)
    {
        List<int> surroundingRoomsList = GetSurroundingCasesList(currentRoom);

        if (surroundingRoomsList.Count == 4)
        {
            if (currentRoom.generationNumber - 1 < 0 && !checkNextInsteadOfPrevious)
            {
                currentRoom = GetCaseFromNumber(checkpointNumber);
                checkNextInsteadOfPrevious = true;
            }
            else
            {
                if (checkNextInsteadOfPrevious)
                {
                    currentRoom = GetCaseFromNumber(currentRoom.generationNumber + 1);
                }
                else
                {
                    currentRoom = GetCaseFromNumber(currentRoom.generationNumber - 1);
                }
            }
            
            return GetNextPosition(currentRoom);
        }
        else
        {
            List<int> availablePositionsList = GetAvailablePositionsListFromUnavailablePositionsList(surroundingRoomsList);

            int selectedPosition = availablePositionsList[Random.Range(0, availablePositionsList.Count)];

            return ConvertIntToPosition(selectedPosition, currentRoom);
        }
    }
    
    List<int> GetSurroundingCasesList(Case targetCase)
    {
        UpdateAllSurroundingCases(targetCase);
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
    
    void UpdateAllSurroundingCases(Case targetCase)
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
    
    List<int> GetAvailablePositionsListFromUnavailablePositionsList(List<int> unavailablePositionsList)
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
    
    void UpdateRoomAppearance()
    {
        foreach (Case room in generationList)
        {
            GetSurroundingCasesList(room);
            room.CloseOutOfBoundsWalls();
        }
    }

    void CleanUpGrid()
    {
        for (int i = 0; i < (2*numberOfRooms+1); i++)
        {
            for (int j = 0; i < (2*numberOfRooms+1); i++)
            {
                generationGrid[i, j] = null;
            }
        }
        
        generationList.Clear();
    }
    
}
