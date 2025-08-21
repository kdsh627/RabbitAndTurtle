using System;
using System.Xml.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Objects/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    private int _level;
    private int _exp;

    private int _maxExp;

    public int Level => _level;
    public int Exp => _exp;
    public int MaxExp => _maxExp;

    public event Action valueChanged;

    public void Init()
    {
        _level = 1;
        _exp = 0;
        UpdateMaxExp();
        valueChanged?.Invoke();
    }

    public void UpdateExp(int value)
    {
        _exp += value;
        if(IsLevelUp())
        {
            _exp = _exp - _maxExp;
            _level++;

            UpdateMaxExp();
            valueChanged?.Invoke();
            UIEventHandler.ToggleSkillEnforceUI_Invoke();
        }
        valueChanged?.Invoke();
    }

    public bool IsLevelUp()
    {
        if(_exp >= _maxExp)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateMaxExp()
    {
        _maxExp = 10 + _level * 3;
    }
}
