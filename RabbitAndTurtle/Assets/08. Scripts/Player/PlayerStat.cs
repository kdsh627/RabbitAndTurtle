using System;
using System.Collections;
using Unity.Behavior.Demo;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public float maxHealth = 100f; // 최대 체력
    public float currentHealth; // 현재 체력
    private PlayerBlock playerBlock;
    private PlayerMovement playerMovement;
    private PlayerAnimationController animatorController;

    public event Action ValueChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBlock = GetComponent<PlayerBlock>();
        playerMovement = GetComponent<PlayerMovement>();
        animatorController = GetComponent<PlayerAnimationController>();
        currentHealth = maxHealth; // 초기 체력 설정
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {
        if(animatorController.isDie)
            return;

        StartCoroutine(playerMovement.DamageAni());
        currentHealth -= damage;
        ValueChanged?.Invoke();
        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        animatorController.isDie = true;
        playerMovement.enabled = false;
        animatorController.PlayDie();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerBlock.isBlock)
        {
            if (other.CompareTag("SongPeyon"))
            {
                float damage = other.GetComponent<EnemyProjectile>().damage;
                TakeDamage(damage);
                Destroy(other.gameObject); // 적 투사체 제거
            }

            if (other.CompareTag("Bbata"))
            {
                CloseEnemyProjectile proj = other.GetComponent<CloseEnemyProjectile>();
                if (proj != null && !proj.isHit)
                {
                    proj.isHit = true; // 중복 데미지 방지
                    TakeDamage(proj.damage);
                }
            }
        }
        
    }
}
