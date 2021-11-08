using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ConsoleManager : MonoBehaviour
{
    private bool showConsole = false;
    [SerializeField] private string input;

    public static ConsoleManager Instance;
    
    private List<object> commandList;
    [SerializeField] private List<string> consoleLines;
    
    private Vector2 scroll;

    private bool scrollToBottom;
    
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
    private static Commands TOGGLENAVMESH;
    private static Commands DESTROYNAVMESH;
    private static Commands TOGGLETELEPORTONGENERATION;
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
        
        UPGRADELIST = new Commands("upgradelist", "Get the list of all upgrades", "upgradelist", () =>
        {
            consoleLines.Add("id - upgrade effect");
        });
        
        GIVEUPGRADE = new Commands<int>("giveupgrade", "Spawns upgrade with corresponding id", "giveupgrade int<upgrade id>", (id) =>
        {
            consoleLines.Add("Spawned upgrade with id "+id);
            
        });
        
        NEWLEVEL = new Commands<int,int>("newlevel", "generates a new run", "newlevel int<number of rooms> int<seed>", (numberOfRooms,seed) =>
        {
            consoleLines.Add("Starting a new run. "+numberOfRooms+" rooms, seed : "+seed);
            
            LevelManager.Instance.StartNewRun(numberOfRooms,seed);
        });
        
        NEXTLEVEL = new Commands("nextlevel", "Generates next level of the current run", "nextlevel", () =>
        {
            LoadingManager.Instance.UpdateLoading();
        
            LevelManager.Instance.GenerateNextLevel();
        });
        
        PREVIOUSLEVEL = new Commands("previouslevel", "Generates previous level of the current run", "previouslevel", () =>
        {
            LoadingManager.Instance.UpdateLoading();
        
            LevelManager.Instance.GeneratePreviousLevel();
        });
        
        TOGGLENAVMESH = new Commands("navmesh", "Toggles Navmesh building during floor generation", "navmesh", () =>
        {
            if (LevelManager.Instance.ToggleNavMesh())
            {
                consoleLines.Add("Navmesh building is now ON");
            }
            else
            {
                consoleLines.Add("Navmesh building is now OFF");
            }
        });
        
        DESTROYNAVMESH = new Commands("clearnavmesh", "Clears Navmesh data", "clearnavmesh", () =>
        {
            consoleLines.Add("Cleared Navmesh data");
            
            LevelManager.Instance.gameObject.GetComponent<NavMeshSurface2d>().RemoveData();
        });
        
        TOGGLETELEPORTONGENERATION = new Commands("teleportongeneration", "Toggles player teleportation on new floor generation (includes when taking portals)", "teleportongeneration", () =>
        {
            if (LevelManager.Instance.ToggleTeleport())
            {
                consoleLines.Add("Teleportation is now ON");
            }
            else
            {
                consoleLines.Add("Teleportation is now OFF");
            }
        });
        
        GETSEED = new Commands("seed", "Get seed of current floor", "seed", () =>
        {
            consoleLines.Add(LevelManager.Instance.GetCurrentSeed() + " (" +
                             LevelManager.Instance.GetCurrentNumberOfRooms() + " rooms)");
        });
        
        GETNUMBEROFROOMS = new Commands("rooms", "Get seed of current floor", "rooms", () =>
        {
            consoleLines.Add(LevelManager.Instance.GetCurrentNumberOfRooms() + " rooms");
        });
        
        GETCURRENTFLOOR = new Commands("floor", "Get floor of current run", "floor", () =>
        {
            consoleLines.Add("Floor "+LevelManager.Instance.GetCurrentFloorNumber() + " of seed "+LevelManager.Instance.GetFirstSeed()+" (" +
                             LevelManager.Instance.GetCurrentNumberOfRooms() + " rooms, first floor is floor 0)");
        });
        
        
        
        
        
        

        commandList = new List<object>()
        {
            HELP,
            CLEARCONSOLE,
            GOTOHUB,
            GOTOLASTROOM,
            GODMODE,
            NOCLIP,
            UPGRADELIST,
            GIVEUPGRADE,
            NEWLEVEL,
            NEXTLEVEL,
            PREVIOUSLEVEL,
            TOGGLENAVMESH,
            DESTROYNAVMESH,
            TOGGLETELEPORTONGENERATION,
            GETSEED,
            GETNUMBEROFROOMS,
            GETCURRENTFLOOR,
            /*
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
            scrollToBottom = true;
            
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

            if (scrollToBottom)
            {
                GUI.ScrollTo(labelRect);
            }
            
            GUI.Label(labelRect,label);
                
            i++;
        }
        scrollToBottom = false;
        
        GUI.EndScrollView(false);

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

        foreach (var commandBase in commandList.Cast<CommandBase>().Where(commandBase => properties[0] == commandBase.commandId))
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
                        Debug.Log("YES");
                        intIntCommand.Invoke(int.Parse(properties[1]),int.Parse(properties[2]));
                    }
                    break;
                    
                default:
                    break;
            }
        }
    }
}

