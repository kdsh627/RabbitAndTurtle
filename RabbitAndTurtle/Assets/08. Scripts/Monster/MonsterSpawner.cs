using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    [Header("타일 및 타겟")]
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Transform target => GameManager.Instance.Player.transform;

    [Header("프리팹 (일반 / 엘리트)")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<GameObject> elitePrefabs; // enemyPrefabs와 인덱스 매핑 1:1 권장
    
    [Header("스폰 설정")]
    [SerializeField] private int initialSpawnCount = 10;
    [SerializeField] private int maxMonsterCount = 40;
    [SerializeField] private int killsPerElite = 10; // n킬마다 엘리트 1마리

    [Header("실행 옵션")]
    [SerializeField] private bool autoStart = false; // true면 Start()에서 자동으로 초기 스폰 + 루프 시작

    [Header("당근 소환")]
    [SerializeField] private GameObject carrotPrefab;   // 기존 Carrots -> carrotPrefab로 사용
    [SerializeField] private int carrotsPerBatch = 3;    // 한 번에 몇 개
    [SerializeField] private float carrotInterval = 30f; // 몇 초마다
    [SerializeField] private int maxCarrots = 30;        // 맵에 동시에 존재 가능한 최대 개수
    [SerializeField] private bool autoStartCarrots = true;

    private readonly List<GameObject> spawnedCarrots = new();
    private Coroutine carrotLoopRoutine;
    private bool allowCarrotSpawns = false;

    private int currentMonsterCount = 0;
    private int[] killCounts; // 일반 몬스터 처치 수

    private List<Vector3> possibleTiles = new List<Vector3>();
    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);

    private bool spawningEnabled = false;  // 스폰 허용 여부
    private bool allowEliteSpawns = true;

    [System.Serializable]
    private struct WayPointData
    {
        public GameObject[] wayPoints;
    }

    [SerializeField] private WayPointData[] wayPointData;

    private Coroutine spawnLoopRoutine;
    private bool initialized = false;

    private void Awake()
    {
        Initialize(); // 초기화만 수행 (스폰/코루틴 시작 X)
    }

    private void Start()
    {
        autoStart = false;
        autoStartCarrots = false;
        spawningEnabled = true; // 스폰 허용
        allowEliteSpawns = true;
        if (!initialized) return;
        if (autoStart)
        {
            SpawnInitialBatch();
            StartSpawnLoop();
        }
        if (autoStartCarrots)
            StartCarrotLoop();
    }

    
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

   
    public void SpawnInitialBatch()
    {
        if (!initialized) return;
        for (int i = 0; i < initialSpawnCount; i++)
            SpawnEnemy();
    }

    public void StartSpawnLoop()
    {
        if (!initialized || spawnLoopRoutine != null || !spawningEnabled) return;
        spawnLoopRoutine = StartCoroutine(SpawnLoop());
    }

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
        while (spawningEnabled)
        {
            if (currentMonsterCount < maxMonsterCount)
                SpawnEnemy();

            float delay = currentMonsterCount < 30 ? 1f : 10f;
            yield return new WaitForSeconds(delay);
        }
        spawnLoopRoutine = null; // 종료 시 핸들 정리
    }

    private void SpawnEnemy()
    {
        if (!spawningEnabled) return;
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

    public void OnMonsterDied(int prefabIndex, bool isElite)
    {
        currentMonsterCount = Mathf.Max(0, currentMonsterCount - 1);

        if (!isElite && allowEliteSpawns)
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


    public void StartSpawning()
    {
        spawningEnabled = true;
        allowEliteSpawns = true;
        StartSpawnLoop();
    }

    public void StopSpawning()
    {
        spawningEnabled = false;
        allowEliteSpawns = false; // 완전 정지면 엘리트도 차단
        StopSpawnLoop();
    }

    public void KillAllMonstersAndStop()
    {
        // 1) 스폰 정지
        StopSpawning();

        BaseMonster[] monsters = Object.FindObjectsByType<BaseMonster>(FindObjectsSortMode.None);

        foreach (var monster in monsters)
        {
            if (!monster.IsDead())
            {
                // 체력을 0으로 만들고 강제로 죽이기
                monster.TakeDamage(monster.MonsterHealth);
            }
        }
    }

    public void StartCarrotLoop()
    {
        allowCarrotSpawns = true;
        if (carrotLoopRoutine == null) carrotLoopRoutine = StartCoroutine(CarrotLoop());
    }

    public void StopCarrotLoop()
    {
        allowCarrotSpawns = false;
        if (carrotLoopRoutine != null)
        {
            StopCoroutine(carrotLoopRoutine);
            carrotLoopRoutine = null;
        }
    }

    private IEnumerator CarrotLoop()
    {
        var wait = new WaitForSeconds(carrotInterval);
        while (allowCarrotSpawns)
        {
            SpawnCarrotBatch();
            yield return wait;
        }
    }

    private void SpawnCarrotBatch()
    {
        if (carrotPrefab == null || possibleTiles.Count == 0) return;

        // 현재 맵에 깔린 수가 max 초과면 추가 스폰 중단
        CleanupCarrotList();
        int canSpawn = Mathf.Min(carrotsPerBatch, maxCarrots - spawnedCarrots.Count);
        if (canSpawn <= 0) return;

        // 한 배치에서 동일 타일 중복 방지
        var used = new HashSet<int>();
        for (int i = 0; i < canSpawn; i++)
        {
            int tries = 20;
            int idx;
            do { idx = Random.Range(0, possibleTiles.Count); }
            while (used.Contains(idx) && --tries > 0);

            used.Add(idx);
            Vector3 pos = possibleTiles[idx];

            var carrot = Instantiate(carrotPrefab, pos, Quaternion.identity);
            spawnedCarrots.Add(carrot);
        }
    }

    private void CleanupCarrotList()
    {
        // 파괴되었거나 비활성화된 오브젝트 정리
        for (int i = spawnedCarrots.Count - 1; i >= 0; i--)
        {
            if (spawnedCarrots[i] == null) spawnedCarrots.RemoveAt(i);
        }
    }
}
