using System.Collections;
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
    private static Command CLEARCONSOLE;
    private static Command HELP;
    
    //General
    private static Command GOTOHUB;
    private static Command GOTOLASTROOM;
    private static Command GODMODE; //Missing some code
    private static Command NOCLIP; //Missing some code
    private static Command UPGRADELIST; //Missing some code
    private static Command<int> GIVEUPGRADE; // Missing some code

    //Level Manger
    private static Command<int,int> NEWLEVEL;
    private static Command NEXTLEVEL;
    private static Command PREVIOUSLEVEL;
    private static Command RELOADLEVEL;
    private static Command TOGGLENAVMESH;
    private static Command DESTROYNAVMESH;
    private static Command TOGGLETELEPORTONGENERATION;
    private static Command GETSEED;
    private static Command GETNUMBEROFROOMS;
    private static Command GETCURRENTFLOOR;
    private static Command<int> SETFIRSTSEED;
    private static Command<int> FORCENEXTSEED;
    private static Command<int> SETNUMBEROFROOMS;
    private static Command GETCURRENTMAXFLOORS;
    private static Command<int> SETMAXFLOORS;
    private static Command GOTOBOSS;
    private static Command WIN;
    

    //Money Manager
    private static Command<int> GIVECOINS; //WORKS ONLY FOR POSITIVE AMOUNT
    private static Command<int> SETCOINS; // NOT IMPLEMENTED
    
    //Hp Manager
    private static Command<int> GIVEHP;
    private static Command<int> SETHP; // Missing some code
    private static Command<int> SETMAXHP; // Missing some code

    //Camera Manager
    private static Command<int> CAMERAMODE; //Missing some code

    //Loading Manager
    private static Command<int> LOADINGMODE;
    
    //Player
    private static Command<float, float> TELEPORTPLAYER;
    private static Command GETPOS;

    //Enemies
    private static Command ENEMYLIST; //Missing some code
    private static Command<int,float,float> SPAWNENEMY; //NEED ENEMY LIST
    private static Command<int, float> DAMAGEENEMY; //Missing code
    private static Command<int, float> KILLENEMY; //Missing code
    private static Command TOGGLESHOWTRIGGERS;

    //Items
    private static Command ITEMLIST; //Missing some code
    private static Command<int,float,float> SPAWNITEM; //NEED ITEM LIST
    private static Command<float> DESTROYITEM; //Missing some code

    //Portal
    private static Command FINDPORTAL;
    private static Command TELEPORTTOPORTAL;
    private static Command<float,float> MOVEPORTAL;
    
    //Spawnpoint
    private static Command FINDSPAWNPOINT;
    private static Command TELEPORTTOSPAWNPOINT;
    private static Command<float, float> MOVESPAWNPOINT;
    
    //Dialogues
    private static Command TOGGLEINSTANTTEXT;

    private LevelManager levelm;
    private LoadingManager loadingm;
    private DialogueManager dialoguem;
    private LifeManager lifem;
    private MoneyManager moneym;
    private bool noclip = false;
    
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
        HELP = new Command("help", "Shows the list of all available commands", "help", () =>
        {
            Print("");
            foreach (CommandBase command in commandList)
            {
                Print($"{command.commandFormat} - {command.commandDescription}");
            }
            Print("");
        });
        
        CLEARCONSOLE = new Command("clear", "Clears console", "clear", () =>
        {
            consoleLines.Clear();
        });
        
        GOTOHUB = new Command("hub", "Teleports to the Hub", "hub", () =>
        {
            Print("Going to Hub");
            
            levelm.GoToHub();
        });
        
        GOTOLASTROOM = new Command("lastroom", "Teleports to last room of the current floor", "lastroom", () =>
        {
            Print("Teleporting to last room");
            
            levelm.MovePlayer(levelm.GetLastRoom().transform);
        });
        
        GODMODE = new Command("godmode", "Toggles godmode", "godmode", () =>
        {
            lifem.isInGodMode = !lifem.isInGodMode;
            Print(lifem.isInGodMode ? "Godmode ON" : "Godmode OFF");
        });
        
        NOCLIP = new Command("noclip", "Toggles noclip", "noclip", () =>
        {
            noclip = !noclip;
            Physics2D.IgnoreLayerCollision(6,13,noclip);
            Print(noclip ? "Noclip ON" : "Noclip OFF");
        });
        
        UPGRADELIST = new Command("upgradelist", "Get the list of all upgrades", "upgradelist", () =>
        {
            Print("id - upgrade effect");
        });
        
        GIVEUPGRADE = new Command<int>("giveupgrade", "Spawns upgrade with corresponding id", "giveupgrade int<upgrade id>", (id) =>
        {
            Print("Spawned upgrade with id "+id);
            
        });
        
        NEWLEVEL = new Command<int,int>("newlevel", "generates a new run", "newlevel int<number of rooms> int<seed>", (numberOfRooms,seed) =>
        {
            levelm.StartNewRun(numberOfRooms,seed);
            
            Print("Starting a new run. "+levelm.GetCurrentNumberOfRooms()+" rooms, seed : "+levelm.GetCurrentSeed());
        });
        
        NEXTLEVEL = new Command("nextlevel", "Generates next level of the current run", "nextlevel", () =>
        {
            loadingm.UpdateLoading();
        
            levelm.Level().GetChild(3).GetComponent<PortalV2>().OnInteraction();
            //levelm.GenerateNextLevel();
        });
        
        PREVIOUSLEVEL = new Command("previouslevel", "Generates previous level of the current run", "previouslevel", () =>
        {
            loadingm.UpdateLoading();
        
            levelm.GeneratePreviousLevel();
        });
        
        RELOADLEVEL = new Command("reloadlevel", "Reloads the current level of the current run", "reloadlevel", () =>
        {
            loadingm.UpdateLoading();
        
            levelm.ReloadLevel();
        });
        
        TOGGLENAVMESH = new Command("navmesh", "Toggles Navmesh building during floor generation", "navmesh", () =>
        {
            Print(levelm.ToggleNavMesh() ? "Navmesh building is now ON" : "Navmesh building is now OFF");
        });
        
        DESTROYNAVMESH = new Command("clearnavmesh", "Clears Navmesh data", "clearnavmesh", () =>
        {
            Print("Cleared Navmesh data");
            
            levelm.gameObject.GetComponent<NavMeshSurface2d>().RemoveData();
        });
        
        TOGGLETELEPORTONGENERATION = new Command("teleportongeneration", "Toggles player teleportation on new floor generation (includes when taking portals)", "teleportongeneration", () =>
        {
            Print(levelm.ToggleTeleport() ? "Teleportation is now ON" : "Teleportation is now OFF");
        });
        
        GETSEED = new Command("seed", "Get seed of current floor", "seed", () =>
        {
            Print(levelm.GetCurrentSeed() + " (" +
                             levelm.GetCurrentNumberOfRooms() + " rooms)");
        });
        
        GETNUMBEROFROOMS = new Command("rooms", "Get seed of current floor", "rooms", () =>
        {
            Print(levelm.GetCurrentNumberOfRooms() + " rooms");
        });
        
        GETCURRENTFLOOR = new Command("floor", "Get floor of current run", "floor", () =>
        {
            Print("Floor "+levelm.GetCurrentFloorNumber() + " of seed "+levelm.GetFirstSeed()+" (" +
                             levelm.GetCurrentNumberOfRooms() + " rooms, first floor is floor 0)");
        });
        
        SETFIRSTSEED = new Command<int>("setseed", "Sets the first seed of the Level Manager", "setseed int<seed>", (seed) =>
        {
            levelm.SetSeedAndRoom(levelm.GetCurrentNumberOfRooms(), seed);
                
            Print("Seed has been set to "+seed);
        });
        
        FORCENEXTSEED = new Command<int>("forceseed", "Forces the seed of the next floor", "forceseed int<seed>", (seed) =>
        {
            levelm.AddSeed(seed);
            
            Print("Next seed will be "+seed);
        });
        
        SETNUMBEROFROOMS = new Command<int>("setnumberofrooms", "Sets the number of rooms for the next generations", "setnumberofrooms int<number of rooms>", (rooms) =>
        {
            levelm.SetSeedAndRoom(rooms,levelm.GetFirstSeed());
            
            Print("Number of rooms has been set to "+rooms+" (will take effect for next generations)");
        });
        
        GETCURRENTMAXFLOORS = new Command("maxfloors", "Get the current number of maximum floors", "maxfloors", () =>
        {
            Print("Max Floors : "+levelm.GetMaxFloors());
        });
        
        SETMAXFLOORS = new Command<int>("setmaxfloors", "Sets the max number of floors", "setmaxfloors int<number of rooms>", (number) =>
        {
            Print("Max number of floors has been set to "+levelm.SetMaxFloors(number)+" (will take effect for next generations)");
        });
        
        GIVECOINS = new Command<int>("addcoins", "Gives a set amount of nyancoins", "addcoins int<amount of coins>", (amount) =>
        {
            if (amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    moneym.PickupCoin();
                }
            }
            
            Print("Added "+amount+" nyancoins");
        });
        
        GOTOBOSS = new Command("gotoboss", "Go to the boss room", "gotoboss", () =>
        {
            levelm.GoToBossFight();
            Print("Going to boss room");
        });
        
        WIN = new Command("win", "Goes to Credit Scene", "win", () =>
        {
            levelm.floorNumber = levelm.GetMaxFloors();
            levelm.Level().GetChild(3).GetComponent<PortalV2>().OnInteraction();
            Print("GG");
        });
        
        SETCOINS = new Command<int>("setcoins", "Sets the amount of nyancoins", "setcoins int<amount of coins>", (amount) =>
        {
            moneym.SetCoins(amount);
            Print("Nyancoins amount set to "+amount);
        });
        
        GIVEHP = new Command<int>("givehp", "Gives a set amount of hp", "givehp int<amount of hp>", (amount) =>
        {
            //increase max hp ?
            
            LifeManager.Instance.TakeDamages(-amount);

            Print("Gave "+amount+" hp");
        });
        
        SETHP = new Command<int>("sethp", "Sets your current amount of hp", "sethp int<amount of hp>", (amount) =>
        {
            Print("Hp set to "+amount+" hp");
        });
        
        SETMAXHP = new Command<int>("setmaxhp", "Sets your current amount of hp", "setmaxhp int<amount of hp>", (amount) =>
        {
            Print("Max Hp set to "+amount+" hp");
        });

        CAMERAMODE = new Command<int>("cameramode", "Sets camera mode (0 follows the player, 1 doesn't", "cameramode int<mode>", (mode) =>
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
        
        LOADINGMODE = new Command<int>("loadingmode", "Changes loading screen display (0 show all, 1 show nothing, 2 no image, 3 no progressbar)", "loadingmode int<mode>", (mode) =>
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

        TELEPORTPLAYER = new Command<float,float>("tp", "Teleports player to coordinates", "tp float<x coordinate> float<y coordinate>", (x,y) =>
        {
            GameObject destination = Instantiate(new GameObject(),new Vector3(x,y,0),Quaternion.identity);

            levelm.MovePlayer(destination.transform);

            Destroy(destination);

            Print("Teleported player to "+x+" "+y);
        });

        GETPOS = new Command("getpos", "Get current player position", "getpos", () =>
        {
            Print("Player is at : "+levelm.GetPos().x+" "+ levelm.GetPos().y);
        });

        ENEMYLIST = new Command("enemylist", "Get the list of all enemies", "enemylist", () =>
        {
            levelm.gameObject.GetComponent<EnemyManager>().GetList();
        });

        SPAWNENEMY = new Command<int,float,float>("spawn", "Summons an enemy", "spawn int<enemy id> float<x coordinate> float<y coordinate>", (id,x,y) =>
        {
            levelm.gameObject.GetComponent<EnemyManager>().SpawnEnemy(id,x,y);
        });
        
        TOGGLESHOWTRIGGERS = new Command("showtriggers", "Toggles rendering of enemy triggers", "showtriggers", () =>
        {
            StartCoroutine(ActivateTriggers());
            
            IEnumerator ActivateTriggers()
            {
                var b = true;
                foreach (Transform enemy in levelm.Level().GetChild(1))
                {
                    foreach (Transform trigger in enemy.GetChild(0).GetChild(0))
                    {
                    
                        if (trigger.gameObject.activeSelf)
                        {
                            trigger.gameObject.GetComponent<EnemyTrigger>().ToggleShowTrigger().ToString();
                        }
                        else
                        {
                            trigger.gameObject.SetActive(true);
                            yield return null;
                            trigger.gameObject.GetComponent<EnemyTrigger>().ToggleShowTrigger().ToString();
                            yield return null;
                            trigger.gameObject.SetActive(false);
                        }
                    }

                    var respawnTrigger = enemy.GetChild(1).gameObject;
                    
                    if (respawnTrigger.activeSelf)
                    {
                        b = respawnTrigger.GetComponent<EnemyTrigger>().ToggleShowTrigger();
                    }
                    else
                    {
                        respawnTrigger.SetActive(true);
                        yield return null;
                        b = respawnTrigger.GetComponent<EnemyTrigger>().ToggleShowTrigger();
                        yield return null;
                        respawnTrigger.SetActive(false);
                    }
                }

                Print(b ? "Trigger rendering is now ON" : "Trigger rendering is now OFF");
            }
        });

        ITEMLIST = new Command("itemlist", "Get the list of all interactible items", "itemlist", () =>
        {
            int id = 1;
            string itemName = "name";
            Print(id + " " + itemName);
        });

        SPAWNITEM = new Command<int, float, float>("item", "Summons an enemy", "item int<enemy id> float<x coordinate> float<y coordinate>", (id, x, y) =>
        {
            //GameObject item = Instantiate(itemlist[id],new Vector3(x, y, 0),Quaternion.identity);

            Print("Spawned " + "item" + " at " + x + " " + y);
        });
        
        DESTROYITEM = new Command<float>("destroy", "Destroy interactable items in range", "destroy float<range>", (range) =>
        {
            int x = 0;
            Print("Destroyed "+x+" items");
        });
        
        FINDPORTAL = new Command("findportal", "Get coordinates of the portal of the current floor", "findportal", () =>
        {
            Vector3 pos = levelm.Level().GetChild(3).position;
            
            Print("Found Portal at "+pos.x+" "+pos.y);
        });
        
        TELEPORTTOPORTAL = new Command("gotoportal", "Teleports player to portal", "gotoportal", () =>
        {
            levelm.MovePlayer(levelm.Level().GetChild(3));
            Print("Teleported Jones to portal");
        });
        
        MOVEPORTAL = new Command<float,float>("moveportal", "Moves portal to coordinates", "moveportal float<x coordinates> float<y coordinates>", (x,y) =>
        {
            levelm.Level().GetChild(3).position = new Vector3(x, y, 0);
            Print("Moved portal to Jones");
        });
        
        FINDSPAWNPOINT = new Command("findstart", "Get coordinates of the start position of the current floor", "findstart", () =>
        {
            var pos = levelm.Level().GetChild(4).position;
            
            Print("Found start position at "+pos.x+" "+pos.y);
        });
        
        TELEPORTTOSPAWNPOINT = new Command("gotostart", "Teleports player to start position", "gotostart", () =>
        {
            levelm.MovePlayer(levelm.Level().GetChild(4));
            Print("Teleported Jones to start position");
        });
        
        MOVESPAWNPOINT = new Command<float,float>("movestart", "Moves start position to coordinates", "movestart float<x coordinates> float<y coordinates>", (x,y) =>
        {
            levelm.Level().GetChild(4).position = new Vector3(x, y, 0);
            Print("Moved start position to Jones");
        });
        
        TOGGLEINSTANTTEXT = new Command("instantdisplay", "Toggles instant display for dialogues", "instantdisplay", () =>
        {
            Print(dialoguem.ToggleInstantTyping() ? "Instant display is now ON" : "Instant display is now OFF");
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
            RELOADLEVEL,
            TOGGLENAVMESH,
            DESTROYNAVMESH,
            TOGGLETELEPORTONGENERATION,
            GETSEED,
            GETNUMBEROFROOMS,
            GETCURRENTFLOOR,
            SETFIRSTSEED,
            FORCENEXTSEED,
            SETNUMBEROFROOMS,
            GETCURRENTMAXFLOORS,
            SETMAXFLOORS,
            GOTOBOSS,
            WIN,
            GIVECOINS,
            SETCOINS,
            GIVEHP,
            SETHP,
            SETMAXHP,
            CAMERAMODE,
            LOADINGMODE,
            TELEPORTPLAYER,
            GETPOS,
            ENEMYLIST,
            SPAWNENEMY,
            TOGGLESHOWTRIGGERS,
            ITEMLIST,
            SPAWNITEM,
            DESTROYITEM,
            FINDPORTAL,
            TELEPORTTOPORTAL,
            MOVEPORTAL,
            FINDSPAWNPOINT,
            TELEPORTTOSPAWNPOINT,
            MOVESPAWNPOINT,
            TOGGLEINSTANTTEXT
        };
    }

    private void Start()
    {
        levelm = LevelManager.Instance;
        loadingm = LoadingManager.Instance;
        dialoguem = DialogueManager.Instance;
        lifem = LifeManager.Instance;
        moneym = MoneyManager.Instance;
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
                    if (commandBase is Command normalCommand)
                    {
                        normalCommand.Invoke();
                    }
                    break;
                    
                case 1:
                    if (commandBase is Command<string> stringCommand)
                    {
                        stringCommand.Invoke(properties[1]);
                    }
                    else if (commandBase is Command<int> intCommand)
                    {
                        intCommand.Invoke(int.Parse(properties[1]));
                    }
                    break;
                    
                case 2:
                    if (commandBase is Command<int,int> intIntCommand)
                    {
                        intIntCommand.Invoke(int.Parse(properties[1]),int.Parse(properties[2]));
                    }
                    else if (commandBase is Command<float, float> CoordsCommand)
                    {
                        CoordsCommand.Invoke(float.Parse(properties[1]), float.Parse(properties[2]));
                    }
                    else if (commandBase is Command<int, float> intFloatCommand)
                    {
                        intFloatCommand.Invoke(int.Parse(properties[1]), float.Parse(properties[2]));
                    }
                    break;

                case 3:
                    if (commandBase is Command<int, float,float> intCoordsCommand)
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
        Debug.Log("CONSOLE: "+message);
    }

}

