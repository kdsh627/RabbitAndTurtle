using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Objects/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    private int _level;
    private int _exp;

    private int _maxExp;

    public event Action valueChanged;

    public void Init()
    {
        _level = 1;
        _exp = 0;
    }

    public void UpdateExp(int value)
    {
        _exp += value;
        if(IsLevelUp())
        {
            _exp = 0;
            _level++;
        }
        valueChanged?.Invoke();
    }

    public bool IsLevelUp()
    {
        if(_exp == _maxExp)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
