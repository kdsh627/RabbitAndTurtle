using System;
using UnityEngine;

public class FlyingFish : MonoBehaviour
{
    [SerializeField] private int _level = 1;
    private int _maxLevel = 5;

    [Header("---- 쿨타임 ----")]
    [SerializeField] private float _maxCoolTime = 3f;
    [SerializeField] private float _coolTime = 0f;
    [SerializeField] private bool isCoolTime = false;

    public float CoolTime => _coolTime;
    public float MaxCoolTime => _maxCoolTime;

    public event Action OnCoolTimeChanged;
    public event Action OnSkillActive;

    void Update()
    {
        if (isCoolTime)
        {
            SkillCoolTime();
        }
    }

    private void SkillCoolTime()
    {
        if (_coolTime > float.Epsilon)
        {
            _coolTime -= Time.deltaTime;

            //여기서 쿨타임 이벤트
            OnCoolTimeChanged?.Invoke();
        }
        else
        {
            //쿨타임 다 돌면
            isCoolTime = false;
            _coolTime = 0f;

            //여기서 활성화 이펙트
            OnSkillActive?.Invoke();
        }
    }

    public void Levelup()
    {
        _level++;
    }

    public bool isMaxLevel()
    {
        return _level == _maxLevel;
    }

    public void QSkill()
    {
        if (isCoolTime) return;

    }
}
