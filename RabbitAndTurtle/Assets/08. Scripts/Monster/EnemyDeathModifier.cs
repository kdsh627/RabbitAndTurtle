using UnityEngine;

public class EnemyDeathNotifier : MonoBehaviour
{
    private MonsterSpawner spawner;
    private int monsterIndex;
    private BaseMonster monster;
    private bool isNotified = false;

    public void Init(MonsterSpawner spawner, int monsterIndex)
    {
        this.spawner = spawner;
        this.monsterIndex = monsterIndex;
    }

    private void Start()
    {
        monster = GetComponent<BaseMonster>();
    }

    private void Update()
    {
        if (isNotified || monster == null) return;

        if (monster.IsDead())
        {
            isNotified = true;
            spawner?.OnMonsterDied(monsterIndex);
            Destroy(this);
        }
    }
}
