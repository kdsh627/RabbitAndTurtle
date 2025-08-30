using System;
using Manager;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public event Action OnWaveStart;
    public event Action WaveValueChanged;

    [Header("---- 태어날 위치 ----")]
    [SerializeField] private Vector3 _startPosition;

    [Header("---- 몬스터 스포너 ----")]
    [SerializeField] private MonsterSpawner _monsterSpawner;

    public void WaveValueChanged_Invoke() => WaveValueChanged?.Invoke();

    private float _waveTime;
    private int _currentWave;
    private int _maxWave;

    private bool _startWave;

    public float WaveTime
    {
        get => _waveTime;
        set => _waveTime = value;
    }

    public int CurrentWave => _currentWave;

    public int MaxWave
    {
        get => _maxWave;
        set => _maxWave = value;
    }

    public void ResetWaveCount()
    {
        _currentWave = 0;
    }

    public void NextWaveCount()
    {
        _currentWave++;
        _startWave = true;
        WaveValueChanged_Invoke();

        OnWaveStart?.Invoke();
        Invoke("StartSpawn", 2f);
    }

    private void StartSpawn()
    {
        _monsterSpawner.SpawnInitialBatch();
        _monsterSpawner.StartSpawnLoop();
        _monsterSpawner.StartCarrotLoop();
    }

    public bool IsStageClear()
    {
        return _currentWave == _maxWave;
    }

    private void UpdateWaveTime()
    {
        if (_waveTime > float.Epsilon)
        {
            _waveTime -= Time.deltaTime;
        }
        else
        {
            _waveTime = 0.0f;
            _startWave = false;
            GameEventHandler.WaveClearExcuted_Invoke();
        }
        WaveValueChanged_Invoke();
    }

    private void Awake()
    {
        GameStateManager.Instace.SetWaveManager(this);
    }

    private void Start()
    {
        GameManager.Instance.Player.transform.position = _startPosition;
        GameEventHandler.StageExcuted_Invoke();
    }

    private void Update()
    {
        if (_startWave)
        {
            UpdateWaveTime();
        }
    }
}
