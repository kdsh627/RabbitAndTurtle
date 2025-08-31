using System;
using DG.Tweening;
using Manager;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public event Action BossValueChanged;

    public event Action OnBossStart;
    public event Action WaveValueChanged;

    [Header("---- 태어날 위치 ----")]
    [SerializeField] private Vector3 _startPosition;

    private float _waveTime;
    private int _currentWave;
    private int _maxWave;

    private bool _startWave;

    private void Awake()
    {
        //GameEventHandler.BossExcuted_Invoke();
        //GameStateManager.Instace.SetWaveManager(this);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.Player.transform.position = _startPosition;
        GameEventHandler.BossExcuted_Invoke();

        Sequence stageClearSequence = DOTween.Sequence();
    }
}
