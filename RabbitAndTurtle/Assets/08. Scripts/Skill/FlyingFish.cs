using System;
using UnityEngine;

public class FlyingFish : MonoBehaviour
{
    [SerializeField] private int _level = 1;
    private int _maxLevel = 5;

    [Header("---- 스킬관련 ----")]
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _monster;

    [Header("---- 쿨타임 ----")]
    [SerializeField] private float _maxCoolTime = 5f;
    [SerializeField] private float _coolTime = 0f;
    [SerializeField] private bool isCoolTime = false;

    [Header("---- 데미지 ----")]
    [SerializeField] public float _damage = 10f;

    public float CoolTime => _coolTime;
    public float MaxCoolTime => _maxCoolTime;

    public GameObject Monster
    {
        get
        {
            return _monster;
        }
        set
        {
            _monster = value;
        }
    }

    public event Action OnCoolTimeChanged;
    public event Action OnSkillActive;
    private void Start()
    {
        OnCoolTimeChanged?.Invoke();
    }

    void Update()
    {
        if(_monster != null)
        {
            transform.position = _monster.transform.position;
        }

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
        _maxCoolTime -= 0.5f;
        _damage += 5f;

        _level++;
    }

    public bool isMaxLevel()
    {
        return _level == _maxLevel;
    }

    public void SkillActive()
    {
        if (isCoolTime) return;

        isCoolTime = true;
        _coolTime = _maxCoolTime;

        _animator.Play("Skill3Active", - 1, 0f);
    }
}
