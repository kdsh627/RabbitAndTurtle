using System;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("---- 레벨 데이터 ----")]
    [SerializeField] private LevelDataSO _levelData;

    private float currentHealth;

    public float maxHealth = 100f; // 최대 체력
    public float CurrentHealth // 현재 체력
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            ValueChanged?.Invoke();
        }
    }
    private PlayerBlock playerBlock;
    private PlayerMovement playerMovement;
    private PlayerAnimationController animatorController;

    public bool isDie = false;

    public event Action ValueChanged;

    void Start()
    {
        playerBlock = GetComponent<PlayerBlock>();
        playerMovement = GetComponent<PlayerMovement>();
        animatorController = GetComponent<PlayerAnimationController>();
        CurrentHealth = maxHealth; // 초기 체력 설정
        isDie = false;
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDie)
            return;

        StartCoroutine(playerMovement.DamageAni());
        CurrentHealth -= damage;
        ValueChanged?.Invoke();
        if (CurrentHealth <= 0) Die();
    }

    public void Die()
    {
        isDie = true;
        playerMovement.enabled = false;
        animatorController.PlayDie();
        Invoke("GameOver", 1f);
    }

    private void GameOver()
    {
        GameEventHandler.GameOverExcuted_Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Exp"))
        {
            Drop drop = other.gameObject.GetComponent<Drop>();
            drop?.GetItem();
            Debug.Log("경험치 습득");
            _levelData.UpdateExp(1);
        }

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
