using RPGM.Gameplay;
using RPGM.UI;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(StateManager))]
public class StateManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StateManager stateMgr = (StateManager)target;
        if (GUILayout.Button("Load Night"))
        {
            stateMgr.StartCoroutine(stateMgr.LoadNight());

        }

        if (GUILayout.Button("Load Day"))
        {
            stateMgr.StartCoroutine(stateMgr.LoadDay());
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
    public int DaySceneIndex = 0;
    public int NightSceneIndex = 1;
    public CharacterController2D Player;
    public GameObject Lantern;
    public Canvas StartMenu;
    public Canvas MainMenu;

    private MemoryStream _savedData = new MemoryStream();

    public IEnumerator LoadDay()
    {
        MainMenu.gameObject.SetActive(true);
        Debug.Log("Saving state");
        SaveState();
        Debug.Log("Closing Night");
        AsyncOperation unload = SceneManager.UnloadSceneAsync(NightSceneIndex);
        while (unload?.isDone == false)
        {
            yield return null;
        }
        Debug.Log("Opening Day");
        SceneManager.LoadScene(DaySceneIndex);
        LoadState();
    }

    public IEnumerator LoadNight()
    {
        StartMenu.gameObject.SetActive(true);
        Debug.Log("Saving state");
        SaveState();
        Debug.Log("Closing Day");
        AsyncOperation unload = SceneManager.UnloadSceneAsync(DaySceneIndex);
        while (unload?.isDone == false)
        {
            yield return null;
        }
        Debug.Log("Opening Night");
        SceneManager.LoadScene(NightSceneIndex);
        LoadState();
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

    public static BinaryFormatter GetBinaryFormatter() {
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
