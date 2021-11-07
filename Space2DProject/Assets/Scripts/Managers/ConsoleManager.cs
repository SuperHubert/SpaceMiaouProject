using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConsoleManager : MonoBehaviour
{
    private bool showConsole = false;
    private bool showHelp = false;
    [SerializeField] private string input;

    public static Commands HELP;
    public static Commands NEXTLEVEL;   
    public static Commands<int,int> NEWLEVEL;
    
    public List<object> commandList;
    
    private Vector2 scroll;


    public void ToggleConsole()
    {
        showConsole = !showConsole;
        input = "";
    }

    public void OnReturn()
    {
        if (showConsole)
        {
            ExecuteInput();
            input = "";
        }
    }

    private void Awake()
    {
        HELP = new Commands("help", "shows the list of all available commands", "help", () =>
        {
            showHelp = true;
        });
        
        NEXTLEVEL = new Commands("nextlevel", "generates next level of the current room", "nextlevel", () =>
        {
            LoadingManager.Instance.UpdateLoading();
        
            LevelManager.Instance.Generate();
        });
        
        NEWLEVEL = new Commands<int,int>("newlevel", "generates a new level", "nextlevel int<number of rooms> int<seed>", (numberOfRooms,seed) =>
        {
            LevelManager.Instance.StartNewRun(numberOfRooms,seed);
        });
        

        commandList = new List<object>()
        {
            HELP,
            NEXTLEVEL,
            NEWLEVEL,
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleConsole();
        }
    }
    
    private void OnGUI()
    {
        if (!showConsole) return;

        float x = Screen.width / 16f;
        float y = Screen.height*3f/4f;
        float width = Screen.width/4f + y;
        float height = 100;

        if (showHelp)
        {
            y -= 100;
            GUI.Box(new Rect(x,y, width,height), "");

            Rect viewport = new Rect(x, 0, width - 30f, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(x, y + 5f, width, height-10f), scroll, viewport);

            int i = 0;
            foreach (CommandBase command in commandList)
            {
                string label = $"{command.commandFormat} - {command.commandDescription}";

                Rect labelRect = new Rect(x+5f, 20 * i, viewport.width - 100, 20);
                
                GUI.Label(labelRect,label);
                
                i++;
            }
            
            GUI.EndScrollView();
            
            y += 100;
        }
        
        GUI.Box(new Rect(x, y, width,30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        
        Event e = Event.current;
        if (e.keyCode == KeyCode.Return)
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

