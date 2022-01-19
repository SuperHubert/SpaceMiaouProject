using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HubConsoleManager : MonoBehaviour
{
    private bool showConsole = false;
    [SerializeField] private string input;

    private List<object> commandList;
    [SerializeField] private List<string> consoleLines;
    
    private Vector2 scroll;

    private bool scrollToBottom;
    
    private static Command HELP;
    private static Command CLEARCONSOLE;
    private static Command RESETDATA;
    private static Command GETSEED;
    private static Command GETNUMBEROFROOMS;
    private static Command GETCURRENTMAXFLOORS;
    private static Command<int> SETSEED;
    private static Command<int> SETNUMBEROFROOMS;
    private static Command<int> SETMAXFLOORS;

    private LoadingLevelData loadingLevelData;

    private void Awake()
    {
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
        
        RESETDATA = new Command("resetdata", "Reset Data (Sets the seed to the current time)", "resetdata", () =>
        {
            loadingLevelData.ResetData(true);
            Print("Data has been reset");
        });
        
        GETSEED = new Command("seed", "Get seed of current floor", "seed", () =>
        {
            Print("seed : "+loadingLevelData.seed);
        });
        
        GETNUMBEROFROOMS = new Command("rooms", "Get number of rooms", "rooms", () =>
        {
            Print("rooms : "+loadingLevelData.numberOfRooms);
        });

        GETCURRENTMAXFLOORS = new Command("maxfloors", "Get the current number of maximum floors", "maxfloors", () =>
        {
            Print("max floors : "+loadingLevelData.maxFloors);
        });
            
        SETSEED = new Command<int>("setseed", "Sets the first seed of the Level Manager", "setseed int<seed>", (seed) =>
        {
            loadingLevelData.seed = seed;
                
            Print("Seed has been set to "+loadingLevelData.seed);
        });
        
        SETNUMBEROFROOMS = new Command<int>("setnumberofrooms", "Sets the number of rooms", "setnumberofrooms int<number of rooms>", (rooms) =>
        {
            loadingLevelData.numberOfRooms = rooms;
                
            Print("Number of rooms has been set to "+loadingLevelData.numberOfRooms);
        });
        
        SETMAXFLOORS = new Command<int>("setmaxfloors", "Sets the max number of floors", "setmaxfloors int<number of rooms>", (number) =>
        {
            loadingLevelData.maxFloors = number;
                
            Print("Max floors has been set to "+loadingLevelData.maxFloors);
        });
        
        commandList = new List<object>()
        {
            HELP,
            CLEARCONSOLE,
            RESETDATA,
            GETSEED,
            GETNUMBEROFROOMS,
            SETSEED,
            SETNUMBEROFROOMS,
            GETCURRENTMAXFLOORS,
            SETMAXFLOORS
        };
    }

    private void Start()
    {
        loadingLevelData = LoadingLevelData.Instance;
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
        if (!showConsole) return;
        Print(input);
            
        ExecuteInput();
        scrollToBottom = true;
            
        input = "";
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
