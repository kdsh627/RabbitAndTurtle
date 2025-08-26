using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseMonster : MonoBehaviour
{
    [Header("스탯")]
    [SerializeField] public float MonsterHealth;
    protected float currentHealth;

    [Header("4방향 스프라이트")]
    [SerializeField] private GameObject FrontSprite;
    [SerializeField] private GameObject BackSprite;
    [SerializeField] private GameObject SideSprite;
    [SerializeField] private GameObject FrontDSprite;
    [SerializeField] private GameObject SideDSprite;

    [Header("옵션")]
    [SerializeField] private bool usePooling = true; // 풀링 사용 여부 (true면 SetActive(false), false면 Destroy)

    [Header("드롭")]
    [SerializeField] public GameObject dropItem;

    private GameObject currentSprite;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer sideDSpriteRenderer;
    private MonsterAnimationController monAni;

    public float moveThreshold = 0.01f;

    private string lastDirection = "Front";
    private BehaviorGraphAgent agent2;
    private NavMeshAgent agent;
    private EnemyFSM fsm;
    public Transform headAnchor;

    private bool isAttacking = false;
    private bool isDead = false;
    private bool isHit = false; // Wave에 맞았는지 여부

    // 추가: 사망 통지자
    private EnemyDeathNotifier deathNotifier;

    protected virtual void Start()
    {
        monAni = GetComponent<MonsterAnimationController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (SideDSprite != null) sideDSpriteRenderer = SideDSprite.GetComponent<SpriteRenderer>();
        agent2 = GetComponent<BehaviorGraphAgent>();
        agent = GetComponent<NavMeshAgent>();
        fsm = GetComponent<EnemyFSM>();

        deathNotifier = GetComponent<EnemyDeathNotifier>();
        if (deathNotifier == null) deathNotifier = gameObject.AddComponent<EnemyDeathNotifier>(); // 안전망

        currentHealth = MonsterHealth;

        if (FrontDSprite) FrontDSprite.SetActive(false);
        if (SideDSprite) SideDSprite.SetActive(false);
    }

    protected virtual void Update()
    {
        if (isDead) return;

        Vector3 velocity = agent != null ? agent.velocity : Vector3.zero;

        if (velocity.magnitude < moveThreshold)
        {
            SetActiveSprite(lastDirection);
            monAni?.PlayIdle(lastDirection);
        }
        else
        {
            // 타겟 기준 바라보기
            if (fsm != null && fsm.target != null)
            {
                Vector3 dirToTarget = fsm.target.position - transform.position;
                string direction = GetDirection(dirToTarget);
                lastDirection = direction;

                SetActiveSprite(direction);

                if (direction == "Side" && spriteRenderer != null && sideDSpriteRenderer != null)
                {
                    bool flip = dirToTarget.x < 0;
                    spriteRenderer.flipX = flip;
                    sideDSpriteRenderer.flipX = flip;
                }

                monAni?.PlayWalk(direction);
            }
        }
    }

    private string GetDirection(Vector3 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) return "Side";
        else return delta.y > 0 ? "Back" : "Front";
    }

    public void EnterAttackMode()
    {
        isAttacking = true;
        FrontSprite?.SetActive(false);
        BackSprite?.SetActive(false);
        SideSprite?.SetActive(false);
    }

    public IEnumerator ExitAttackMode()
    {
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
        SetActiveSprite(lastDirection);
    }

    private void SetActiveSprite(string direction)
    {
        if (isAttacking) return;

        FrontSprite?.SetActive(false);
        BackSprite?.SetActive(false);
        SideSprite?.SetActive(false);

        switch (direction)
        {
            case "Front": currentSprite = FrontSprite; break;
            case "Back": currentSprite = BackSprite; break;
            case "Side": currentSprite = SideSprite; break;
        }

        currentSprite?.SetActive(true);
    }

    public abstract void Attack();

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        DamageManager.Instance.Show(damage, transform.position + Vector3.up * 0.5f, fsm.MinDamage, fsm.MaxDamage);

        StartCoroutine(DamageAni());
        currentHealth -= damage;

        if (currentHealth <= 0f)
            Die();
    }

    IEnumerator DamageAni()
    {
        if (agent != null) agent.isStopped = true;

        if (lastDirection == "Front" || lastDirection == "Back" || currentHealth <= 0)
        {
            if (FrontDSprite) FrontDSprite.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            if (FrontDSprite) FrontDSprite.SetActive(false);
        }
        else
        {
            if (SideDSprite) SideDSprite.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            if (SideDSprite) SideDSprite.SetActive(false);
        }

        if (!isDead && agent != null) agent.isStopped = false;
    }

    public bool IsDead() => currentHealth <= 0f;

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        // 이동/AI 정지
        FrontSprite.SetActive(false);
        BackSprite.SetActive(false);
        SideSprite.SetActive(false);
        FrontDSprite.SetActive(false); ;
        SideDSprite.SetActive(false);

        HardStopAI();

        deathNotifier?.NotifyDeath();

        // 연출 후 회수/파괴
        StartCoroutine(DieAni());
        StartCoroutine(SpawndropItemDelay());
    }

    IEnumerator DieAni()
    {
        monAni?.PlayDie();
        yield return new WaitForSeconds(1f);

        if (usePooling)
        {
            // 풀링: 비활성화해서 풀로 반환(풀 구현에 맞춰 OnDisable에서 회수하거나 외부 매니저가 감시)
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator SpawndropItemDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(dropItem, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("SongPeyon"))
        {
            if (collision.TryGetComponent<EnemyProjectile>(out EnemyProjectile projectile))
            {
                if (projectile.isReflected)
                {
                    TakeDamage(projectile.damage);
                    Destroy(projectile.gameObject);
                }
            }
            return;
        }

        if (collision.CompareTag("Wave") && !isHit)
        {
            if (collision.TryGetComponent<Wave>(out Wave wave))
            {
                isHit = true;
                TakeDamage(wave.damage);
                StartCoroutine(WaveDmg());
            }
        }
    }

    IEnumerator WaveDmg()
    {
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    private void HardStopAI()
    {
        // 1) 에이전트 완전 정지
        if (agent != null)
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
            agent.velocity = Vector3.zero;

            agent.enabled = false;
        }

        if (agent2 != null) agent2.enabled = false; // BehaviorGraphAgent
        if (fsm != null) fsm.enabled = false;    // EnemyFSM(목표 재설정 방지)

        // 3) 혹시 물리 이동 쓰면 같이 정지 (있는 경우에만)
        var rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null) { rb2d.linearVelocity = Vector2.zero; rb2d.angularVelocity = 0f; }

        var animator = GetComponent<Animator>();
        if (animator != null) animator.applyRootMotion = false;
    }
}
