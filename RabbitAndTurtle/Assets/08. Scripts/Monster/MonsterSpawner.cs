using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    [Header("타일 및 타겟")]
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Transform target;

    [Header("프리팹 (일반 / 엘리트)")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<GameObject> elitePrefabs; // enemyPrefabs와 인덱스 매핑 1:1 권장

    [Header("스폰 설정")]
    [SerializeField] private int initialSpawnCount = 10;
    [SerializeField] private int maxMonsterCount = 40;
    [SerializeField] private int killsPerElite = 10; // n킬마다 엘리트 1마리

    [Header("실행 옵션")]
    [SerializeField] private bool autoStart = true; // true면 Start()에서 자동으로 초기 스폰 + 루프 시작

    private int currentMonsterCount = 0;
    private int[] killCounts; // 일반 몬스터 처치 수

    private List<Vector3> possibleTiles = new List<Vector3>();
    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);

    [System.Serializable]
    private struct WayPointData
    {
        public GameObject[] wayPoints;
    }

    [SerializeField] private WayPointData[] wayPointData;

    private Coroutine spawnLoopRoutine;
    private bool initialized = false;

    // ---------------------------
    // 라이프사이클
    // ---------------------------
    private void Awake()
    {
        Initialize(); // 초기화만 수행 (스폰/코루틴 시작 X)
    }

    private void Start()
    {
        if (!initialized) return;
        if (autoStart)
        {
            SpawnInitialBatch();
            StartSpawnLoop();
        }
    }

    // ---------------------------
    // 초기화(데이터 준비) - 웨이브에서 재사용 가능
    // ---------------------------
    public void Initialize()
    {
        if (initialized) return;

        if (tileMap == null)
        {
            Debug.LogError("[MonsterSpawner] Tilemap이 할당되지 않았어.");
            enabled = false;
            return;
        }

        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.LogError("[MonsterSpawner] enemyPrefabs 비어있음");
            enabled = false;
            return;
        }

        tileMap.CompressBounds();
        CalculatePossibleTiles();

        killCounts = new int[enemyPrefabs.Count];
        currentMonsterCount = 0;

        initialized = true;
    }

    private void CalculatePossibleTiles()
    {
        possibleTiles.Clear();

        BoundsInt bounds = tileMap.cellBounds;
        TileBase[] allTiles = tileMap.GetTilesBlock(bounds);

        for (int y = 1; y < bounds.size.y - 1; y++)
        {
            for (int x = 1; x < bounds.size.x - 1; x++)
            {
                TileBase tile = allTiles[y * bounds.size.x + x];
                if (tile != null)
                {
                    Vector3Int local = bounds.position + new Vector3Int(x, y);
                    Vector3 pos = tileMap.CellToWorld(local) + offset;
                    pos.z = 0;
                    possibleTiles.Add(pos);
                }
            }
        }

        if (possibleTiles.Count == 0)
            Debug.LogWarning("[MonsterSpawner] 스폰 가능한 타일이 하나도 없어.");
    }

    // ---------------------------
    // 스폰 제어(웨이브에서 호출)
    // ---------------------------
    /// <summary>초기 몬스터 패킷 스폰(한 번)</summary>
    public void SpawnInitialBatch()
    {
        if (!initialized) return;
        for (int i = 0; i < initialSpawnCount; i++)
            SpawnEnemy();
    }

    /// <summary>지속 스폰 루프 시작</summary>
    public void StartSpawnLoop()
    {
        if (!initialized) return;
        if (spawnLoopRoutine != null) return;
        spawnLoopRoutine = StartCoroutine(SpawnLoop());
    }

    /// <summary>지속 스폰 루프 정지</summary>
    public void StopSpawnLoop()
    {
        if (spawnLoopRoutine != null)
        {
            StopCoroutine(spawnLoopRoutine);
            spawnLoopRoutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (currentMonsterCount < maxMonsterCount)
                SpawnEnemy();

            float delay = currentMonsterCount < 30 ? 1f : 10f;
            yield return new WaitForSeconds(delay);
        }
    }

    // ---------------------------
    // 개별 스폰 로직
    // ---------------------------
    private void SpawnEnemy()
    {
        if (possibleTiles.Count == 0) return;
        if (currentMonsterCount >= maxMonsterCount) return;

        int tileIndex = Random.Range(0, possibleTiles.Count);
        int wayIndex = (wayPointData != null && wayPointData.Length > 0)
            ? Random.Range(0, wayPointData.Length)
            : -1;

        int prefabIndex = Random.Range(0, enemyPrefabs.Count);

        // 일반 몬스터는 풀에서 꺼냄(없으면 Instantiate)
        GameObject clone = MonsterPool.Instance.Get(prefabIndex);
        if (clone == null)
            clone = Instantiate(enemyPrefabs[prefabIndex]);

        clone.transform.position = possibleTiles[tileIndex];

        var fsm = clone.GetComponent<EnemyFSM>();
        if (fsm != null && wayIndex >= 0)
            fsm.Setup(target, wayPointData[wayIndex].wayPoints);

        var notifier = clone.GetComponent<EnemyDeathNotifier>();
        if (notifier == null) notifier = clone.AddComponent<EnemyDeathNotifier>();
        notifier.Init(this, prefabIndex, false); // 일반 몬스터

        currentMonsterCount++;
    }

    private void SpawnElite(int index)
    {
        if (possibleTiles.Count == 0) return;
        if (currentMonsterCount >= maxMonsterCount) return;

        if (elitePrefabs == null || index < 0 || index >= elitePrefabs.Count)
        {
            Debug.LogWarning($"[MonsterSpawner] elitePrefabs에 index {index} 없음");
            return;
        }

        int tileIndex = Random.Range(0, possibleTiles.Count);
        int wayIndex = (wayPointData != null && wayPointData.Length > 0)
            ? Random.Range(0, wayPointData.Length)
            : -1;

        GameObject elite = Instantiate(elitePrefabs[index]);
        elite.transform.position = possibleTiles[tileIndex];

        var fsm = elite.GetComponent<EnemyFSM>();
        if (fsm != null && wayIndex >= 0)
            fsm.Setup(target, wayPointData[wayIndex].wayPoints);

        var notifier = elite.GetComponent<EnemyDeathNotifier>();
        if (notifier == null) notifier = elite.AddComponent<EnemyDeathNotifier>();
        notifier.Init(this, index, true); // 엘리트

        currentMonsterCount++;
    }

    // ---------------------------
    // 콜백(사망 통지)
    // ---------------------------
    public void OnMonsterDied(int prefabIndex, bool isElite)
    {
        currentMonsterCount = Mathf.Max(0, currentMonsterCount - 1);

        if (!isElite)
        {
            if (prefabIndex < 0 || prefabIndex >= killCounts.Length)
            {
                Debug.LogWarning($"[MonsterSpawner] 잘못된 prefabIndex {prefabIndex}");
                return;
            }

            killCounts[prefabIndex]++;

            if (killsPerElite > 0 && killCounts[prefabIndex] % killsPerElite == 0)
            {
                SpawnElite(prefabIndex);
            }
        }
    }
}
