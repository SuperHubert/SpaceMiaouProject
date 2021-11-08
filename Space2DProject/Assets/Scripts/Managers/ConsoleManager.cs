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
    private static Commands GODMODE; //Missing some code
    private static Commands NOCLIP; //Missing some code
    private static Commands UPGRADELIST; //Missing some code
    private static Commands<int> GIVEUPGRADE; // Missing some code

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
    private static Commands<int> FORCENEXTSEED;
    private static Commands<int> SETNUMBEROFROOMS;

    //Money Manager
    private static Commands<int> GIVECOINS; //WORKS ONLY FOR POSITIVE AMOUNT
    private static Commands<int> SETCOINS; // Missing some code
    
    //Hp Manager
    private static Commands<int> GIVEHP;
    private static Commands<int> SETHP; // Missing some code
    private static Commands<int> SETMAXHP; // Missing some code

    //Camera Manager
    private static Commands<int> CAMERAMODE; //Missing some code
    private static Commands TOGGLECAMERALOCK; //Missing some code
    
    //Loading Manager
    private static Commands TRUELOADINGIMAGE; //Missing some code
    private static Commands<int> LOADINGMODE;
    
    //Player
    private static Commands<float, float> TELEPORTPLAYER;
    private static Commands GETPOS;

    //Enemies
    private static Commands ENEMYLIST; //Missing some code
    private static Commands<int,float,float> SPAWNENEMY; //NEED ENEMY LIST
    private static Commands<int, float> DAMAGEENEMY; //Missing code
    private static Commands<int, float> KILLENEMY; //Missing code

    //Items
    private static Commands ITEMLIST; //Missing some code
    private static Commands<int,float,float> SPAWNITEM; //NEED ITEM LIST
    private static Commands<float> DESTROYITEM; //Missing some code

    //Portal
    private static Commands FINDPORTAL;
    private static Commands TELEPORTTOPORTAL;
    private static Commands<float,float> MOVEPORTAL;
    
    //Spawnpoint
    private static Commands FINDSPAWNPOINT;
    private static Commands TELEPORTTOSPAWNPOINT;
    private static Commands<float, float> MOVESPAWNPOINT;
    
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
        
        #region Commands
        HELP = new Commands("help", "Shows the list of all available commands", "help", () =>
        {
            Print("");
            foreach (CommandBase command in commandList)
            {
                Print($"{command.commandFormat} - {command.commandDescription}");
            }
            Print("");
        });
        
        CLEARCONSOLE = new Commands("clear", "Clears console", "clear", () =>
        {
            consoleLines.Clear();
        });
        
        GOTOHUB = new Commands("hub", "Teleports to the Hub", "hub", () =>
        {
            Print("Going to Hub");
            
            LevelManager.Instance.GoToHub();
        });
        
        GOTOLASTROOM = new Commands("lastroom", "Teleports to last room of the current floor", "lastroom", () =>
        {
            Print("Teleporting to last room");
            
            LevelManager.Instance.MovePlayer(LevelManager.Instance.GetLastRoom().transform);
        });
        
        GODMODE = new Commands("godmode", "Toggles godmode", "godmode", () =>
        {
            if (true)
            {
                Print("Godmode ON");
            }
            else
            {
                Print("Godmode OFF");
            }
        });
        
        NOCLIP = new Commands("noclip", "Toggles noclip", "noclip", () =>
        {
            if (true)
            {
                Print("Noclip ON");
            }
            else
            {
                Print("Noclip OFF");
            }
        });
        
        UPGRADELIST = new Commands("upgradelist", "Get the list of all upgrades", "upgradelist", () =>
        {
            Print("id - upgrade effect");
        });
        
        GIVEUPGRADE = new Commands<int>("giveupgrade", "Spawns upgrade with corresponding id", "giveupgrade int<upgrade id>", (id) =>
        {
            Print("Spawned upgrade with id "+id);
            
        });
        
        NEWLEVEL = new Commands<int,int>("newlevel", "generates a new run", "newlevel int<number of rooms> int<seed>", (numberOfRooms,seed) =>
        {
            Print("Starting a new run. "+numberOfRooms+" rooms, seed : "+seed);
            
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
                Print("Navmesh building is now ON");
            }
            else
            {
                Print("Navmesh building is now OFF");
            }
        });
        
        DESTROYNAVMESH = new Commands("clearnavmesh", "Clears Navmesh data", "clearnavmesh", () =>
        {
            Print("Cleared Navmesh data");
            
            LevelManager.Instance.gameObject.GetComponent<NavMeshSurface2d>().RemoveData();
        });
        
        TOGGLETELEPORTONGENERATION = new Commands("teleportongeneration", "Toggles player teleportation on new floor generation (includes when taking portals)", "teleportongeneration", () =>
        {
            if (LevelManager.Instance.ToggleTeleport())
            {
                Print("Teleportation is now ON");
            }
            else
            {
                Print("Teleportation is now OFF");
            }
        });
        
        GETSEED = new Commands("seed", "Get seed of current floor", "seed", () =>
        {
            Print(LevelManager.Instance.GetCurrentSeed() + " (" +
                             LevelManager.Instance.GetCurrentNumberOfRooms() + " rooms)");
        });
        
        GETNUMBEROFROOMS = new Commands("rooms", "Get seed of current floor", "rooms", () =>
        {
            Print(LevelManager.Instance.GetCurrentNumberOfRooms() + " rooms");
        });
        
        GETCURRENTFLOOR = new Commands("floor", "Get floor of current run", "floor", () =>
        {
            Print("Floor "+LevelManager.Instance.GetCurrentFloorNumber() + " of seed "+LevelManager.Instance.GetFirstSeed()+" (" +
                             LevelManager.Instance.GetCurrentNumberOfRooms() + " rooms, first floor is floor 0)");
        });
        
        SETFIRSTSEED = new Commands<int>("setseed", "Sets the first seed of the Level Manager", "setseed int<seed>", (seed) =>
        {
            LevelManager.Instance.SetSeedAndRoom(LevelManager.Instance.GetCurrentNumberOfRooms(), seed);
                
            Print("Seed has been set to "+seed);
        });
        
        FORCENEXTSEED = new Commands<int>("forceseed", "Forces the seed of the next floor", "forceseed int<seed>", (seed) =>
        {
            LevelManager.Instance.AddSeed(seed);
            
            Print("Next seed will be "+seed);
        });
        
        SETNUMBEROFROOMS = new Commands<int>("setnumberofrooms", "Sets the number of rooms for the next generations", "setnumberofrooms int<number of rooms>", (rooms) =>
        {
            LevelManager.Instance.SetSeedAndRoom(rooms,LevelManager.Instance.GetFirstSeed());
            
            Print("Number of rooms has been set to "+rooms+" (will take effect for next generations");
        });
        
        GIVECOINS = new Commands<int>("addcoins", "Gives a set amount of nyancoins", "addcoins int<amount of coins>", (amount) =>
        {
            if (amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    MoneyManager.Instance.PickupCoin();
                }
            }
            
            Print("Added "+amount+" nyancoins");
        });
        
        SETCOINS = new Commands<int>("setcoins", "Sets the amount of nyancoins", "setcoins int<amount of coins>", (amount) =>
        {
            Print("Nyancoins amount set to "+amount);
        });
        
        GIVEHP = new Commands<int>("givehp", "Gives a set amount of hp", "givehp int<amount of hp>", (amount) =>
        {
            //increase max hp ?
            
            LifeManager.Instance.TakeDamages(-amount);

            Print("Gave "+amount+" hp");
        });
        
        SETHP = new Commands<int>("sethp", "Sets your current amount of hp", "sethp int<amount of hp>", (amount) =>
        {
            Print("Hp set to "+amount+" hp");
        });
        
        SETMAXHP = new Commands<int>("setmaxhp", "Sets your current amount of hp", "setmaxhp int<amount of hp>", (amount) =>
        {
            Print("Max Hp set to "+amount+" hp");
        });

        CAMERAMODE = new Commands<int>("cameramode", "Sets camera mode (0 follows the player, 1 doesn't", "cameramode int<mode>", (mode) =>
        {
            //ChangeCameraMode(mode)
            switch (mode)
            {
                case 0:
                    Print("Cameramode switched to 0 (follows player)");
                    break;

                case 1:
                    Print("Cameramode switched to 1 (follows player)");
                    break;

                default:
                    Print("invalid mode : " + mode);
                    break;
            }
        
        });

        TOGGLECAMERALOCK = new Commands("cameralock", "Toggles camera controls", "cameralock", () =>
        {
            //ToggleCameraControls()
            if (true)
            {
                Print("Camera controls are now ON");
            }
            else
            {
                Print("Camera controls are now OFF");
            }
        });

        TRUELOADINGIMAGE = new Commands("trueloadingimage", "Toggles true loading image", "trueloadingimage", () =>
         {
             //ToggleLoadingImage()
             if (true)
             {
                 Print("True image is now ON");
             }
             else
             {
                 Print("True image is now OFF");
             }
         });

        LOADINGMODE = new Commands<int>("loadingmode", "Changes loading screen display (0 show all, 1 show nothing, 2 no image, 3 no progressbar)", "loadingmode int<mode>", (mode) =>
        {
            
            switch (LoadingManager.Instance.ChangeLoadingMode(mode))
            {
                case 0:
                    Print("Loadingmode switched to 0 (show all)");
                    break;

                case 1:
                    Print("Loadingmode switched to 1 (show nothing)");
                    break;

                case 2:
                    Print("Loadingmode switched to 2 (no image)");
                    break;

                case 3:
                    Print("Loadingmode switched to 3 (no progress bar)");
                    break;

                default:
                    Print("invalid mode : " + mode);
                    break;
            }
        });

        TELEPORTPLAYER = new Commands<float,float>("tp", "Teleports player to coordinates", "tp float<x coordinate> float<y coordinate>", (x,y) =>
        {
            GameObject destination = Instantiate(new GameObject(),new Vector3(x,y,0),Quaternion.identity);

            LevelManager.Instance.MovePlayer(destination.transform);

            Destroy(destination);

            Print("Teleported player to "+x+" "+y);
        });

        GETPOS = new Commands("getpos", "Get current player position", "getpos", () =>
        {
            Print("Jones is at : "+LevelManager.Instance.GetPos().x+" "+ LevelManager.Instance.GetPos().y);
        });

        ENEMYLIST = new Commands("enemylist", "Get the list of all enemies", "enemylist", () =>
        {
            int id = 1;
            string enemyName = "name";
            Print(id + " "+enemyName);
        });

        SPAWNENEMY = new Commands<int,float,float>("spawn", "Summons an enemy", "spawn int<enemy id> float<x coordinate> float<y coordinate>", (id,x,y) =>
        {
            //GameObject enemy = Instantiate(enemylist[id],new Vector3(x, y, 0),Quaternion.identity);

            Print("Spawned "+"enemyName"+" at " + x + " " + y);
        });

        ITEMLIST = new Commands("itemlist", "Get the list of all interactible items", "itemlist", () =>
        {
            int id = 1;
            string itemName = "name";
            Print(id + " " + itemName);
        });

        SPAWNITEM = new Commands<int, float, float>("item", "Summons an enemy", "item int<enemy id> float<x coordinate> float<y coordinate>", (id, x, y) =>
        {
            //GameObject item = Instantiate(itemlist[id],new Vector3(x, y, 0),Quaternion.identity);

            Print("Spawned " + "item" + " at " + x + " " + y);
        });
        
        DESTROYITEM = new Commands<float>("destroy", "Destroy interactable items in range", "destroy float<range>", (range) =>
        {
            int x = 0;
            Print("Destroyed "+x+" items");
        });
        
        FINDPORTAL = new Commands("findportal", "Get coordinates of the portal of the current floor", "findportal", () =>
        {
            Vector3 pos = LevelManager.Instance.Level().GetChild(3).position;
            
            Print("Found Portal at "+pos.x+" "+pos.y);
        });
        
        TELEPORTTOPORTAL = new Commands("gotoportal", "Teleports player to portal", "gotoportal", () =>
        {
            LevelManager.Instance.MovePlayer(LevelManager.Instance.Level().GetChild(3));
            Print("Teleported Jones to portal");
        });
        
        MOVEPORTAL = new Commands<float,float>("moveportal", "Moves portal to coordinates", "moveportal float<x coordinates> float<y coordinates>", (x,y) =>
        {
            LevelManager.Instance.Level().GetChild(3).position = new Vector3(x, y, 0);
            Print("Moved portal to Jones");
        });
        
        FINDSPAWNPOINT = new Commands("findstart", "Get coordinates of the start position of the current floor", "findstart", () =>
        {
            Vector3 pos = LevelManager.Instance.Level().GetChild(4).position;
            
            Print("Found start position at "+pos.x+" "+pos.y);
        });
        
        TELEPORTTOSPAWNPOINT = new Commands("gotostart", "Teleports player to start position", "gotostart", () =>
        {
            LevelManager.Instance.MovePlayer(LevelManager.Instance.Level().GetChild(4));
            Print("Teleported Jones to start position");
        });
        
        MOVESPAWNPOINT = new Commands<float,float>("movestart", "Moves start position to coordinates", "movestart float<x coordinates> float<y coordinates>", (x,y) =>
        {
            LevelManager.Instance.Level().GetChild(4).position = new Vector3(x, y, 0);
            Print("Moved start position to Jones");
        });
        #endregion


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
            SETFIRSTSEED,
            FORCENEXTSEED,
            SETNUMBEROFROOMS,
            GIVECOINS,
            SETCOINS,
            GIVEHP,
            SETHP,
            SETMAXHP,
            CAMERAMODE,
            TOGGLECAMERALOCK,
            TRUELOADINGIMAGE,
            LOADINGMODE,
            TELEPORTPLAYER,
            GETPOS,
            ENEMYLIST,
            SPAWNENEMY,
            //DAMAGEENEMY,
            //KILLENEMY,
            ITEMLIST,
            SPAWNITEM,
            DESTROYITEM,
            FINDPORTAL,
            TELEPORTTOPORTAL,
            MOVEPORTAL,
            FINDSPAWNPOINT,
            TELEPORTTOSPAWNPOINT,
            MOVESPAWNPOINT
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
            Print(input);
            
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

            Rect labelRect = new Rect(x+5f, 20 * i, viewport.width - 10f, 20);

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
        input = input.Replace(".", ",");

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
                        intIntCommand.Invoke(int.Parse(properties[1]),int.Parse(properties[2]));
                    }
                    else if (commandBase is Commands<float, float> CoordsCommand)
                    {
                        CoordsCommand.Invoke(float.Parse(properties[1]), float.Parse(properties[2]));
                    }
                    else if (commandBase is Commands<int, float> intFloatCommand)
                    {
                        intFloatCommand.Invoke(int.Parse(properties[1]), float.Parse(properties[2]));
                    }
                    break;

                case 3:
                    if (commandBase is Commands<int, float,float> intCoordsCommand)
                    {
                        intCoordsCommand.Invoke(int.Parse(properties[1]), float.Parse(properties[2]), float.Parse(properties[3]));
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public void Print(string message)
    {
        consoleLines.Add(message);
    }

}
