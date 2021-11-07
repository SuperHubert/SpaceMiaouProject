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
    public static Commands TEST;
    public static Commands TEST2;
    public static Commands TEST4;
    public static Commands<string> TEST3;

    public List<object> commandList;


    public void ToggleConsole()
    {
        showConsole = !showConsole;
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
        TEST = new Commands("test", "a test", "test", () =>
        {
            Debug.Log("test");
        });
        TEST2 = new Commands("yoink", "a test", "test", () =>
        {
            Debug.Log("test2");
        });
        TEST4 = new Commands("efe", "a test", "test", () =>
        {
            Debug.Log("test2");
        });
        TEST3 = new Commands<string>("set_gold", "a test", "set_gold <Gold Amount>", (x) =>
        {
            Debug.Log("Amount : "+x);
        });

        commandList = new List<object>()
        {
            HELP,
            TEST,
            TEST2,
            TEST3,
            TEST4
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleConsole();
        }
    }


    private Vector2 scroll;
    
    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0f;

        if (showHelp)
        {
            GUI.Box(new Rect(0,y, Screen.width,100), "");

            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

            int i = 0;
            foreach (CommandBase command in commandList)
            {
                string label = $"{command.commandFormat} - {command.commandDescription}";

                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                
                GUI.Label(labelRect,label);
                
                i++;
            }
            
            GUI.EndScrollView();
            
            y += 100;
        }
        
        GUI.Box(new Rect(0, y, Screen.width,30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        
        Event e = Event.current;
        if (e.keyCode == KeyCode.Return)
        {
            OnReturn();
        }
        else
        {
            input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        }
    }

    private void ExecuteInput()
    {
        string[] properties = input.Split(' ');
        
        foreach (CommandBase commandBase in commandList)
        {
            if (input.Contains(commandBase.commandId))
            {
                if (commandBase is Commands normalCommand)
                {
                    normalCommand.Invoke();
                }
                else if (commandBase is Commands<string> stringCommand)
                {
                    stringCommand.Invoke(properties[1]);
                }
                else if (commandBase is Commands<int> intCommand)
                {
                    intCommand.Invoke(int.Parse(properties[1]));
                }
            }
        }
    }
}

