using RPGM.Gameplay;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CustomEditor(typeof(StateManager))]
public class StateManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StateManager stateMgr = (StateManager)target;
        if (GUILayout.Button("Load Night"))
        {
            stateMgr.LoadNight();
        }

        if (GUILayout.Button("Load Day"))
        {
            stateMgr.LoadDay();
        }

        if (GUILayout.Button("Open Main Menu"))
        {
            stateMgr.OpenMainMenu();
        }

        if (GUILayout.Button("Open Start Menu"))
        {
            stateMgr.OpenStartMenu();
        }
    }
}

public class StateManager : MonoBehaviour
{
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
    public CharacterController2D Player;
    public GameObject Lantern;
    public Canvas StartMenu;
    public Canvas MainMenu;
    public Light2D globalLight;
    public Color nightColor = new Color(.16f, .16f, .8f);
    public Color dayColor = Color.white;

    private MemoryStream _savedData = new MemoryStream();

    public void OpenMainMenu()
    {
        MainMenu.gameObject.SetActive(true);
    }

    public void OpenStartMenu()
    {
        StartMenu.gameObject.SetActive(true);
    }

    public void LoadDay()
    {
        Debug.Log("Day Time");
        globalLight.color = dayColor;
    }

    public void LoadNight()
    {
        Debug.Log("Night Time");
        globalLight.color = nightColor;
    }

    void Awake()
    {
        StartMenu.gameObject.SetActive(true);
        DontDestroyOnLoad(gameObject);
    }

    public void CloseMainMenu()
    {
        MainMenu.gameObject.SetActive(false);
        StartMenu.gameObject.SetActive(false);
    }

    public void SaveState()
    {
        BinaryFormatter formatter = GetBinaryFormatter();
        GameState gameState = new GameState
        {
            Tombstones = Tombstones,
            Lantern = Lantern.GetComponent<Transform>().position,
            Player = Player.GetComponent<Transform>().position,
        };
    }

    public void LoadState()
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
