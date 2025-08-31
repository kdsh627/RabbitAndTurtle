using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private int _maxHP;
    [SerializeField] private int _hp;

    public int MaxHP => _maxHP;
    public int HP => _hp;

    public event Action OnValueChanged; 

    private void Awake()
    {
        _hp = _maxHP;

        OnValueChanged?.Invoke();
    }

    private void DecreaseHP(int damage)
    {
        _hp = _maxHP;

        OnValueChanged?.Invoke();
    }
}
