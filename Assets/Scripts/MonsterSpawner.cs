using JetBrains.Annotations;
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

        if(GUILayout.Button("Destroy Monster"))
        {
            Debug.Log("Destory Activated");
            Monster monster = spawner.MonsterInstance?.GetComponent<Monster>();
            if (monster != null) {
                monster.DestroyMonster();
            }
            else
            {
                Debug.LogWarning("Unable to find Monster");
            }
        }
    }
}

public class MonsterSpawner : MonoBehaviour
{
    public Transform Target;
    public GameObject MonsterPrefab;

    [HideInInspector]
    public GameObject MonsterInstance;

    public void SpawnMonster()
    {
        if (transform == null)
        {
            Debug.LogError("Please add this script to the scene");
            return;
        }
        if (MonsterInstance != null)
        {
            Debug.LogError("Spawners handle one monster at a time.");
            return;
        }
        MonsterInstance = Instantiate(MonsterPrefab, transform, true);
    }

    public void MoveMonster()
    {
        Monster monsterScrpt = MonsterInstance?.GetComponent<Monster>();
        if (monsterScrpt == null)
        {
            SpawnMonster();
            monsterScrpt = MonsterInstance?.GetComponent<Monster>();
            if (monsterScrpt == null)
            {
                Debug.LogError("No monster spawned to move.");
                return;
            }
        }

        if (Target == null)
        {
            Debug.LogError("Need destination for monster");
            StartCoroutine(monsterScrpt.DestroyMonster());
            return;
        }
        //ToDo: Don't use flat time, do math
        iTween.MoveTo(MonsterInstance, Target.position, 2f);
    }
}