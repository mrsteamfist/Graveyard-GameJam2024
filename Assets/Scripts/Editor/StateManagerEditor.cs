using UnityEditor;
using UnityEngine;

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
