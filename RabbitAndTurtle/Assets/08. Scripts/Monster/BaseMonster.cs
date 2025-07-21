using UnityEngine;

public abstract class BaseMonster : MonoBehaviour
{
    [Header("스탯")]
    [SerializeField] public float MonsterHealth;
    protected float currentHealth;

    [SerializeField] public float MonsterSpeed;
    [SerializeField] public float MonsterDamage;
    [SerializeField] public float MonsterAttackSpeed;

    [Header("공격")]
    [SerializeField] public GameObject MonsterAttackRange;

    protected bool isPlayerInRange = false;
    protected float attackCooldown = 0f;
    protected Transform targetPlayer;

    protected virtual void Start()
    {
        currentHealth = MonsterHealth;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            targetPlayer = player.transform;
        }
    }

    protected virtual void Update()
    {
        if (targetPlayer != null)
        {
            MoveTowardsPlayer();

            if (isPlayerInRange)
            {
                attackCooldown -= Time.deltaTime;
                if (attackCooldown <= 0f)
                {
                    Attack();
                    attackCooldown = MonsterAttackSpeed;
                }
            }
        }
    }


    public virtual void MoveTowardsPlayer()
    {
        if (targetPlayer != null)
        {
            Vector2 direction = (targetPlayer.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, MonsterSpeed * Time.deltaTime); 
        }
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            targetPlayer = collision.transform;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            targetPlayer = null;
        }
    }
}
