using UnityEngine;

public abstract class BaseMonster : MonoBehaviour
{
    [Header("스탯")]
    [SerializeField] public float MonsterHealth;
    protected float currentHealth;

    [Header("공격")]
    [SerializeField] public GameObject MonsterAttackRange;

    protected virtual void Start()
    {
        currentHealth = MonsterHealth;
    }

    // 자식에서 재정의할 함수
    public abstract void Attack();

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Monster has died.");
        Destroy(gameObject);
    }
}
