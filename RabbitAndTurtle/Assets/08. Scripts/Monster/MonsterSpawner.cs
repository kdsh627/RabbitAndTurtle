using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    [Header("타일 및 타겟")]
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Transform target;

    [Header("프리팹")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<GameObject> elitePrefabs;

    [Header("스폰 설정")]
    [SerializeField] private int initialSpawnCount = 10;
    [SerializeField] private int maxMonsterCount = 40;

    private int currentMonsterCount = 0;
    private int[] killCounts;

    private List<Vector3> possibleTiles = new List<Vector3>();
    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);

    [System.Serializable]
    private struct WayPointData
    {
        public GameObject[] wayPoints;
    }

    [SerializeField]
    private WayPointData[] wayPointData;

    private void Awake()
    {
        tileMap.CompressBounds();
        CalculatePossibleTiles();

        killCounts = new int[enemyPrefabs.Count];

        // 초기 10마리 소환
        for (int i = 0; i < initialSpawnCount; i++)
            SpawnEnemy();

        // 지속 스폰 루프
        StartCoroutine(SpawnLoop());
    }

    private void CalculatePossibleTiles()
    {
        BoundsInt bounds = tileMap.cellBounds;
        TileBase[] allTiles = tileMap.GetTilesBlock(bounds);

        for (int y = 1; y < bounds.size.y - 1; y++)
        {
            for (int x = 1; x < bounds.size.x - 1; x++)
            {
                TileBase tile = allTiles[y * bounds.size.x + x];
                if (tile != null)
                {
                    Vector3Int localPosition = bounds.position + new Vector3Int(x, y);
                    Vector3 position = tileMap.CellToWorld(localPosition) + offset;
                    position.z = 0;
                    possibleTiles.Add(position);
                }
            }
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (currentMonsterCount < maxMonsterCount)
            {
                SpawnEnemy();
            }

            float delay = currentMonsterCount < 30 ? 1f : 10f;
            yield return new WaitForSeconds(delay);
        }
    }

    private void SpawnEnemy()
    {
        if (possibleTiles.Count == 0 || currentMonsterCount >= maxMonsterCount)
            return;

        int tileIndex = Random.Range(0, possibleTiles.Count);
        int wayIndex = Random.Range(0, wayPointData.Length);
        int prefabIndex = Random.Range(0, enemyPrefabs.Count);

        GameObject clone = MonsterPool.Instance.Get(prefabIndex);
        if (clone == null) return;

        clone.transform.position = possibleTiles[tileIndex];
        clone.GetComponent<EnemyFSM>().Setup(target, wayPointData[wayIndex].wayPoints);

        // 사망 처리 등록
        EnemyDeathNotifier notifier = clone.GetComponent<EnemyDeathNotifier>();
        if (notifier == null)
            notifier = clone.AddComponent<EnemyDeathNotifier>();

        notifier.Init(this, prefabIndex);


        currentMonsterCount++;
    }

        public void OnMonsterDied(int prefabIndex)
    {
        currentMonsterCount--;

        killCounts[prefabIndex]++;
        if (killCounts[prefabIndex] % 10 == 0 && prefabIndex < elitePrefabs.Count)
        {
            SpawnElite(prefabIndex);
        }
    }

    private void SpawnElite(int index)
    {
        int tileIndex = Random.Range(0, possibleTiles.Count);
        int wayIndex = Random.Range(0, wayPointData.Length);

        GameObject elite = MonsterPool.Instance.Get(index);
        if (elite == null) return;

        elite.transform.position = possibleTiles[tileIndex];
        elite.GetComponent<EnemyFSM>().Setup(target, wayPointData[wayIndex].wayPoints);

        EnemyDeathNotifier notifier = elite.GetComponent<EnemyDeathNotifier>();
        if (notifier == null)
            notifier = elite.AddComponent<EnemyDeathNotifier>();

        notifier.Init(this, index);

        currentMonsterCount++;
    }

}
