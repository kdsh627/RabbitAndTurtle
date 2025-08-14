using System;
using Manager;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public event Action WaveValueChanged;
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
    }

    public bool IsStageClear()
    {
        return _currentWave == _maxWave;
    }
    
    private void UpdateWaveTime()
    {
        if (_waveTime >= float.Epsilon)
        {
            _waveTime -= Time.deltaTime;
        }
        else
        {
            _waveTime = 0.0f;
            _startWave = false;
        }
        WaveValueChanged_Invoke();
    }

    private void Awake()
    {
        GameStateManager.Instace.SetWaveManager(this);
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
