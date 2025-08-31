using System;
using System.Collections;
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
    public bool noDamage = false;

    public event Action ValueChanged;
    [SerializeField] GameObject ItemEffect; // 아이템 획득 이펙트
    public int  HealAmount = 0;

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
        if (!noDamage)
        {
            if (isDie)
                return;
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.TurtleHurt);
            StartCoroutine(playerMovement.DamageAni());
            CurrentHealth -= damage;
            ValueChanged?.Invoke();
            if (CurrentHealth <= 0) Die();
        }
    }

    public virtual void Heal(float amount)
    {
        if (isDie)
            return;
        if(CurrentHealth >= maxHealth) 
            return;
        CurrentHealth += amount;
        ValueChanged?.Invoke();
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

    private IEnumerator ItemEffectCor()
    {
        ItemEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ItemEffect.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Exp"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.EatCarrot);
            Drop drop = other.gameObject.GetComponent<Drop>();
            drop?.GetItem();
            StartCoroutine(ItemEffectCor());
            Debug.Log("경험치 습득");
            _levelData.UpdateExp(1);
        }

        if (other.gameObject.CompareTag("Carrot"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.EatCarrot);
            Drop drop = other.gameObject.GetComponent<Drop>();
            drop?.GetItem();
            StartCoroutine(ItemEffectCor());
            Heal(HealAmount); 
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
                    playerMovement.KnockBack(proj.dir);
                    Vector2 hitPos = other.transform.position;
                }
            }
        }

    }
}
