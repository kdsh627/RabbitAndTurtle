using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public float maxHealth = 100f; // 최대 체력
    public float currentHealth; // 현재 체력

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth; // 초기 체력 설정
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {

    }
}
