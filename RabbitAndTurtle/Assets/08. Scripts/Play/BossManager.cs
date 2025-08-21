using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public event Action BossValueChanged;

    private float _waveTime;
    private int _currentWave;
    private int _maxWave;

    private bool _startWave;


    private void Awake()
    {
        GameEventHandler.BossExcuted_Invoke();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
