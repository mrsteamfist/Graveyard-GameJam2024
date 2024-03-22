using Graveyard;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StateManager : MonoBehaviour
{
    public enum GameStates
    {
        Unset,
        DayScreen,
        Day,
        NightScreen,
        Night,
        Win,
        Lost
    }

    private static StateManager _stateManager;

    [HideInInspector]
    public static StateManager Instance
    {
        get
        {
            if (_stateManager == null)
            {
                _stateManager = FindFirstObjectByType<StateManager>();
            }
            return _stateManager;
        }
    }

    public Tombstone[] Tombstones;
    public MonsterSpawner[] Spawners;
    public ControllerAPI Player;
    public GameObject Lantern;
    public Canvas DayMenu;
    public Canvas NightMenu;
    public Canvas GameWin;
    public Canvas GameOver;
    public Light2D globalLight;
    public Color nightColor = new Color(.16f, .16f, .8f);
    public Color dayColor = Color.white;
    public bool AllowMenus = false;
    public int dayCycleTimeSec = 10;
    public int nightCycleTimeSec = 10;
    public int currentScore = 0;
    public int spawnProbability = 100;
    public int spawnGap = 1;

    public GameStates CurrentState
    {
        get { return _currentState; }
        set
        {
            lastProcessedGameState = _currentState;
            _currentState = value;
        }
    }

    private DateTime lastSpawnTime = DateTime.MinValue;
    private System.Random rng = new System.Random();
    private GameStates lastProcessedGameState = GameStates.Unset;
    private GameStates _currentState = GameStates.Unset;
    private DateTime stateStart = DateTime.MinValue;
    private MemoryStream _savedData = new MemoryStream();

    public void OpenMainMenu()
    {
        NightMenu.gameObject.SetActive(true && AllowMenus);
    }

    public void OpenStartMenu()
    {
        DayMenu.gameObject.SetActive(true && AllowMenus);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadDay()
    {
        Debug.Log("Day Time");
        CurrentState = GameStates.Day;
        stateStart = DateTime.Now;
        this.Player.HasLantern = false;
        globalLight.color = dayColor;
        Lantern.SetActive(false);
    }

    public void LoadNight()
    {
        Debug.Log("Night Time");
        CurrentState = GameStates.Night;
        stateStart = DateTime.Now;
        globalLight.color = nightColor;
        Lantern.SetActive(true);
    }

    void Awake()
    {
        if (CurrentState == GameStates.Unset)
        {
            CurrentState = GameStates.DayScreen;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        if (CurrentState == GameStates.Night && rng.Next(spawnProbability) == 1 && (DateTime.Now - lastSpawnTime).TotalSeconds > spawnGap)
        {
            lastSpawnTime = DateTime.Now;
            int spawnAt = rng.Next(Spawners.Length);
            Spawners[spawnAt].SpawnMonster();
            Spawners[spawnAt].MoveMonster();
        }

        bool didWin = true;
        bool didLose = true;
        foreach (var tombstone in Tombstones)
        {
            if (tombstone.TombstoneState != Tombstone.State.Destroy)
            {
                didLose = false;
            }
            if (tombstone.TombstoneState != Tombstone.State.Grow)
            {
                didWin = false;
            }
            if (!didWin && !didLose)
            {
                break;
            }
        }

        if (didWin)
        {
            CurrentState = GameStates.Win;
        }
        else if (didLose)
        {
            CurrentState = GameStates.Lost;
        }
        else if (lastProcessedGameState != GameStates.Unset && lastProcessedGameState == CurrentState)
        {
            if (CurrentState == GameStates.Day && (DateTime.Now - stateStart).TotalSeconds > dayCycleTimeSec)
            {
                Debug.Log("Time is Up " + (DateTime.Now - stateStart).TotalSeconds);
                CurrentState = GameStates.NightScreen;
            }
            else if (CurrentState == GameStates.Night && (DateTime.Now - stateStart).TotalSeconds > nightCycleTimeSec)
            {
                Debug.Log("Time is Up " + (DateTime.Now - stateStart).TotalSeconds);
                CurrentState = GameStates.DayScreen;
            }
            else
            {
                return;
            }
        }

        stateStart = DateTime.Now;

        switch (CurrentState)
        {
            case GameStates.DayScreen:
                if (AllowMenus)
                {
                    Debug.Log("Showing day screen");
                    DayMenu.gameObject.SetActive(true);
                }
                else
                    LoadDay();
                break;
            case GameStates.NightScreen:
                if (AllowMenus)
                {
                    Debug.Log("Showing day screen");
                    NightMenu.gameObject.SetActive(true);
                }
                else
                    LoadNight();
                break;
            case GameStates.Day:
                LoadDay();
                CloseMainMenu();
                break;
            case GameStates.Night:
                LoadNight();
                CloseMainMenu();
                break;
            case GameStates.Lost:
                GameOver.gameObject.SetActive(true);
                break;
            case GameStates.Win:
                GameWin.gameObject.SetActive(true);
                break;
        }
    }

    public void CloseMainMenu()
    {
        NightMenu.gameObject.SetActive(false);
        DayMenu.gameObject.SetActive(false);
        if (CurrentState == GameStates.DayScreen)
            CurrentState = GameStates.Day;
        else if (CurrentState == GameStates.NightScreen)
            CurrentState = GameStates.Night;
    }

    public void SaveGame()
    {
        BinaryFormatter formatter = GetBinaryFormatter();
        GameState gameState = new GameState
        {
            Tombstones = Tombstones,
            Lantern = Lantern.GetComponent<Transform>().position,
            Player = Player.GetComponent<Transform>().position,
        };
    }

    public void LoadSaveGame()
    {
        BinaryFormatter formatter = GetBinaryFormatter();
        GameState gameState = (GameState)formatter.Deserialize(_savedData);

        for (int i = 0; i < Tombstones.Length && i < gameState.Tombstones.Length; i++)
        {
            Tombstones[i].LoadSaved(gameState.Tombstones[i]);
        }
        Player.GetComponent<Transform>().position = gameState.Player;
        Lantern.GetComponent<Transform>().position = gameState.Lantern;
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SurrogateSelector selector = new SurrogateSelector();

        LocationSerializer lSerializer = new LocationSerializer();
        TombestoneSerializer tSeralizer = new TombestoneSerializer();

        selector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), lSerializer);
        selector.AddSurrogate(typeof(Tombstone), new StreamingContext(StreamingContextStates.All), tSeralizer);

        formatter.SurrogateSelector = selector;

        return formatter;
    }

}
