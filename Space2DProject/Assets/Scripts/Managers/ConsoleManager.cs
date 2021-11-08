using System;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    private bool showConsole = false;
    [SerializeField] private string input;

    public static ConsoleManager Instance;
    
    private List<object> commandList;
    [SerializeField] private List<string> consoleLines;
    
    private Vector2 scroll;
    //console
    private static Commands CLEARCONSOLE;
    private static Commands HELP;
    
    //General
    private static Commands GOTOHUB;
    private static Commands GOTOLASTROOM;
    private static Commands GODMODE; //NOT IMPLEMENTED
    private static Commands NOCLIP; //NOT IMPLEMENTED
    private static Commands UPGRADELIST; //NOT IMPLEMENTED
    private static Commands<int> GIVEUPGRADE; // NOT IMPLEMENTED

    //Level Manger
    private static Commands<int,int> NEWLEVEL;
    private static Commands NEXTLEVEL;
    private static Commands PREVIOUSLEVEL;
    private static Commands<string> BUILDNAVMESH;
    private static Commands<string> TELEPORTONNEWLEVELSPAWN;
    private static Commands GETSEED;
    private static Commands GETNUMBEROFROOMS;
    private static Commands GETCURRENTFLOOR;
    private static Commands<int> SETFIRSTSEED;
    private static Commands<int> SETNUMBEROFROOMS;

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
        #region Singleton Don't Destroy On Load
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
        
        HELP = new Commands("help", "Shows the list of all available commands", "help", () =>
        {
            consoleLines.Add("");
            foreach (CommandBase command in commandList)
            {
                consoleLines.Add($"{command.commandFormat} - {command.commandDescription}");
            }
            consoleLines.Add("");
        });
        
        CLEARCONSOLE = new Commands("clear", "Clears console", "clear", () =>
        {
            consoleLines.Clear();
        });
        
        GOTOHUB = new Commands("hub", "Teleports to the Hub", "hub", () =>
        {
            consoleLines.Add("Going to Hub");
            
            LevelManager.Instance.GoToHub();
        });
        
        GOTOLASTROOM = new Commands("lastroom", "Teleports to last room of the current floor", "lastroom", () =>
        {
            consoleLines.Add("Teleporting to last room");
            
            LevelManager.Instance.MovePlayer(LevelManager.Instance.GetLastRoom().transform);
        });
        
        GODMODE = new Commands("godmode", "Toggles godmode", "godmode", () =>
        {
            if (true)
            {
                consoleLines.Add("Godmode ON");
            }
            else
            {
                consoleLines.Add("Godmode OFF");
            }
        });
        
        NOCLIP = new Commands("noclip", "Toggles noclip", "noclip", () =>
        {
            if (true)
            {
                consoleLines.Add("Noclip ON");
            }
            else
            {
                consoleLines.Add("Noclip OFF");
            }
        });
        
        GIVEUPGRADE = new Commands<int>("giveupgrade", "Spawns upgrade with corresponding id", "giveupgrade int<upgrade id>", (id) =>
        {
            consoleLines.Add("Spawned upgrade with id "+id);
            
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
            /*
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
            SETFIRSTSEED,
            SETNUMBEROFROOMS,
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
            */
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

