using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConsoleManager : MonoBehaviour
{
    private bool showConsole = false;
    [SerializeField] private string input;
    
    private List<object> commandList;
    [SerializeField] private List<string> consoleLines;
    
    private Vector2 scroll;
    //console
    private static Commands CLEARCONSOLE;
    private static Commands HELP;
    
    //General
    private static Commands GOTOHUB;
    private static Commands GOTOLASTROOM;
    private static Commands GODMODE;
    private static Commands NOCLIP;
    private static Commands<int> GIVEUPGRADE;

    //Level Manger
    private static Commands<int,int> NEWLEVEL;
    private static Commands NEXTLEVEL;
    private static Commands PREVIOUSLEVEL;
    private static Commands<string> BUILDNAVMESH;
    private static Commands<string> TELEPORTONNEWLEVELSPAWN;
    private static Commands GETSEED;
    private static Commands GETNUMBEROFROOMS;
    private static Commands GETCURRENTFLOOR;

    //Money Manager
    private static Commands<int> GIVECOINS;
    private static Commands<int> SETCOINS;
    
    //Hp Manager
    private static Commands<int> GIVEHP;
    private static Commands<int> SETHP;

    //Camera Manager
    private static Commands<string> CAMERAFOLLOW;
    private static Commands<string> CAMERALOCK;
    
    //Loading Manager
    private static Commands<string> TRUELOADINGIMAGE;
    
    //Player
    private static Commands<int, int> TELEPORTPLAYER;
    
    //Enemies
    private static Commands<int> SPAWNENEMY;
    private static Commands<int, float> DAMAGEENEMY;
    private static Commands<int, float> KILLENEMY;

    //Items
    private static Commands<int> SPAWNITEM;
    private static Commands<float> DESTROYITEM;

    //Portal
    private static Commands FINDPORTAL;
    private static Commands TELEPORTTOPORTAL;
    private static Commands TELEPORTPORTAL;
    
    //Spawnpoint
    private static Commands FINDSPAWNPOINT;
    private static Commands TELEPORTTOSPAWNPOINT;
    
    private void Awake()
    {
        HELP = new Commands("help", "shows the list of all available commands", "help", () =>
        {
            consoleLines.Add("");
            foreach (CommandBase command in commandList)
            {
                consoleLines.Add($"{command.commandFormat} - {command.commandDescription}");
            }
            consoleLines.Add("");
        });
        
        CLEARCONSOLE = new Commands("clear", "clears console", "clear", () =>
        {
            consoleLines.Clear();
        });
        
        NEXTLEVEL = new Commands("nextlevel", "generates next level of the current room", "nextlevel", () =>
        {
            LoadingManager.Instance.UpdateLoading();
        
            LevelManager.Instance.Generate();
        });
        
        NEWLEVEL = new Commands<int,int>("newlevel", "generates a new level", "nextlevel int<number of rooms> int<seed>", (numberOfRooms,seed) =>
        {
            consoleLines.Add("Starting a new run. "+numberOfRooms+" rooms, seed : "+seed);
            
            LevelManager.Instance.StartNewRun(numberOfRooms,seed);
        });
        
        
        
        
        

        commandList = new List<object>()
        {
            HELP,
            CLEARCONSOLE,
            GOTOHUB,
            GOTOLASTROOM,
            GODMODE,
            NOCLIP,
            GIVEUPGRADE,
            NEWLEVEL,
            NEXTLEVEL,
            PREVIOUSLEVEL,
            BUILDNAVMESH,
            TELEPORTONNEWLEVELSPAWN,
            GETSEED,
            GETNUMBEROFROOMS,
            GETCURRENTFLOOR,
            GIVECOINS,
            SETCOINS,
            GIVEHP,
            SETHP,
            CAMERAFOLLOW,
            CAMERALOCK,
            TRUELOADINGIMAGE,
            TELEPORTPLAYER,
            SPAWNENEMY,
            DAMAGEENEMY,
            KILLENEMY,
            SPAWNITEM,
            DESTROYITEM,
            FINDPORTAL,
            TELEPORTTOPORTAL,
            TELEPORTPORTAL,
            FINDSPAWNPOINT,
            TELEPORTTOSPAWNPOINT
        };
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleConsole();
        }
    }
    
    private void ToggleConsole()
    {
        showConsole = !showConsole;
        input = "";
    }

    private void OnReturn()
    {
        if (showConsole)
        {
            consoleLines.Add(input);
            
            ExecuteInput();
            
            input = "";
        }
    }
    
    private void OnGUI()
    {
        if (!showConsole) return;
        
        float x = Screen.width / 16f;
        float y = Screen.height*3f/4f;
        float width = Screen.width/4f + y;
        float height = 200;
        
        string label;
        
        y -= height;
        GUI.Box(new Rect(x,y, width,height), "");
        
        Rect viewport = new Rect(x, 0, width - 30f, 20 * consoleLines.Count);

        scroll = GUI.BeginScrollView(new Rect(x, y + 5f, width, height-10f), scroll, viewport);
        
        int i = 0;
        foreach (string line  in consoleLines)
        {
            label = $"{line}";

            Rect labelRect = new Rect(x+5f, 20 * i, viewport.width - height, 20);
            
            GUI.ScrollTo(labelRect);
                
            GUI.Label(labelRect,label);
                
            i++;
        }
        
        GUI.EndScrollView();

        y += height;
        
        GUI.Box(new Rect(x, y, width,30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        
        Event e = Event.current;
        if (e.keyCode == KeyCode.Return && input != "")
        {
            OnReturn();
        }
        else
        {
            input = GUI.TextField(new Rect(x+10f, y + 5f, width - 20f, 20f), input);
        }
    }

    private void ExecuteInput()
    {
        string[] properties = input.Split(' ');
        
        Debug.Log(properties.Length);
        
        foreach (CommandBase commandBase in commandList)
        {
            if (input.Contains(commandBase.commandId))
            {
                switch (properties.Length - 1)
                {
                    case 0:
                        if (commandBase is Commands normalCommand)
                        {
                            normalCommand.Invoke();
                        }
                        break;
                    
                    case 1:
                        if (commandBase is Commands<string> stringCommand)
                        {
                            stringCommand.Invoke(properties[1]);
                        }
                        else if (commandBase is Commands<int> intCommand)
                        {
                            intCommand.Invoke(int.Parse(properties[1]));
                        }
                        break;
                    
                    case 2:
                        if (commandBase is Commands<int,int> intIntCommand)
                        {
                            intIntCommand.Invoke(int.Parse(properties[1]),int.Parse(properties[2]));
                        }
                        break;
                    
                    default:
                        ConsoleLog("console too long");
                        break;
                }
                
                
                
                
                
            }
        }
    }

    private void ConsoleLog(string message)
    {
        Debug.Log(message);
    }
}

