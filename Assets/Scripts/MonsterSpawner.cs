using UnityEngine;

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
        MonsterInstance.GetComponent<Monster>().OnDeath += MonsterSpawner_OnDeath;
    }

    private void MonsterSpawner_OnDeath(object sender, System.EventArgs e)
    {
        MonsterInstance = null;
        
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
