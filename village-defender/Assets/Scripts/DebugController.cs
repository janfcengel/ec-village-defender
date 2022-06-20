using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject timeContoller;

    bool showDebug;
    bool showHelp; 
    string input;
    Vector2 scroll; 


    public static DebugCommand PLAYER_HEALTH_FULL_LIFE;
    public static DebugCommand<int> PLAYER_HEALTH_SET_TO;
    public static DebugCommand<int> TIME_SET;
    public static DebugCommand HELP;

    public List<object> commandList;

    public void OnToggleDebugConsole(InputValue value)
    {
        showDebug = !showDebug; 
    }

    public void OnReturn(InputValue value)
    {
        if (showDebug)
        {
            HandleInput();
            input = ""; 
        }
    }

    public void Awake()
    {
        PLAYER_HEALTH_FULL_LIFE = new DebugCommand("player_health_full_life", "Heals the players health to 100.", "player_health_full_life", () =>
        {
            HealthBarSystem healthBar = player.GetComponent<PlayerController>().getHealthBar();
            healthBar.AddHealth(healthBar.GetMaxHealth());
            Debug.Log("Debug Console: Player health set to " + healthBar.GetMaxHealth() + ".");
        });

        PLAYER_HEALTH_SET_TO = new DebugCommand<int>("player_health_set_to", "Sets the player health to given argument", "player_health_set_to <int>", (value) => {
            HealthBarSystem healthBar = player.GetComponent<PlayerController>().getHealthBar();
            healthBar.SetHealth(value);
            Debug.Log("Debug Console: Player health set to " + value + ".");
        });

        TIME_SET = new DebugCommand<int>("time_set", "Sets the time to given argument", "time_set <int>", (value) => {
            TimeController tc = timeContoller.GetComponent<TimeController>();
            tc.SetTime(value);
            Debug.Log("Debug Console: Time set to " + value + ":00.");
        });

        HELP = new DebugCommand("help", "Toggle help for available commands", "help", () =>
        {
            showHelp = !showHelp; 
        });


    commandList = new List<object>
        {
            PLAYER_HEALTH_FULL_LIFE,
            PLAYER_HEALTH_SET_TO,
            TIME_SET,
            HELP,
        };
    }
    private void OnGUI()
    {
        if(!showDebug) { return; }

        float y = 0f;

        if(showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);
            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);
            for(int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.commandFormat} - {command.commandDescription}";
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                GUI.Label(labelRect, label);
            }
            GUI.EndScrollView();
            y += 100; 
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input); 
    }

    private void HandleInput()
    {
        string[] arguments = input.Split(' ');

        for(int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if(input.Contains(commandBase.commandId))
            {
                if(commandList[i] as DebugCommand != null)
                { 
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if(commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(arguments[1]));
                }
            }
        }
    }
}
