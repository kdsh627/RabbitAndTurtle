using UnityEngine;

public class EnemyDeathNotifier : MonoBehaviour
{
    private MonsterSpawner spawner;
    private int typeIndex;
    private bool isElite;
    private bool sent; // 중복 통지 방지

    public void Init(MonsterSpawner spawner, int typeIndex, bool isElite)
    {
        this.spawner = spawner;
        this.typeIndex = typeIndex;
        this.isElite = isElite;
        sent = false;
    }

    /// <summary>
    /// 적이 실제로 죽을 때(체력 0 처리 시) 한 번만 호출해줘.
    /// </summary>
    public void NotifyDeath()
    {
        if (sent) return;
        sent = true;

        if (spawner != null)
            spawner.OnMonsterDied(typeIndex, isElite);
    }

    private void OnDisable()
    {
        // 풀 재사용 대비: 다음 회차에서 다시 통지 가능해야 하므로 리셋
        sent = false;
    }
}
