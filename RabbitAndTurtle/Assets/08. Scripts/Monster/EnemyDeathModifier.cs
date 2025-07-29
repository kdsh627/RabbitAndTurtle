using UnityEngine;

public class EnemyDeathNotifier : MonoBehaviour
{
    private MonsterSpawner spawner;
    private int index;

    public void Init(MonsterSpawner spawner, int index)
    {
        this.spawner = spawner;
        this.index = index;
    }

    public void NotifyDeath()
    {
        spawner.OnMonsterDied(index);
        MonsterPool.Instance.Return(index, gameObject);
    }
}
