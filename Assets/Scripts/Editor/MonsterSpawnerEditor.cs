using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonsterSpawner))]
public class MonsterSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MonsterSpawner spawner = (MonsterSpawner)target;
        if (GUILayout.Button("Spawn Monster"))
        {
            Debug.Log("Spawn Activated");
            spawner.SpawnMonster();
        }

        if (GUILayout.Button("Move Monster"))
        {
            Debug.Log("Move Activated");
            spawner.MoveMonster();
        }

        if (GUILayout.Button("Destroy Monster"))
        {
            Debug.Log("Destory Activated");
            Monster monster = spawner.MonsterInstance?.GetComponent<Monster>();
            if (monster != null)
            {
                monster.DestroyMonster();
            }
            else
            {
                Debug.LogWarning("Unable to find Monster");
            }
        }
    }
}
