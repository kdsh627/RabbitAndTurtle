using NUnit.Framework.Constraints;
using System.Collections;
using Unity.Behavior;
using Unity.Behavior.Demo;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

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

    private GameObject currentSprite;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer sideDSpriteRenderer;
    private MonsterAnimationController monAni;
    public float moveThreshold = 0.01f;

    private Vector3 lastPosition;
    private string lastDirection = "Front";
    private BehaviorGraphAgent agent2;
    private NavMeshAgent agent;
    private BlackboardVariable var;
    private EnemyFSM fsm;
    private bool isAttacking = false;
    private bool isDead = false;

    protected virtual void Start()
    { 
        monAni = GetComponent<MonsterAnimationController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sideDSpriteRenderer = SideDSprite.GetComponent<SpriteRenderer>();
        agent2 = GetComponent<BehaviorGraphAgent>();
        agent = GetComponent<NavMeshAgent>();
        fsm = GetComponent<EnemyFSM>();

        currentHealth = MonsterHealth;
        lastPosition = transform.position;
        FrontDSprite.SetActive(false);
        SideDSprite.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!isDead)
        {
            Vector3 velocity = agent.velocity;

            if (velocity.magnitude < moveThreshold)
            {
                SetActiveSprite(lastDirection);
                monAni.PlayIdle(lastDirection);
            }
            else
            {
                // 움직였으면 target 기준으로 바라보게 설정
                Vector3 dirToTarget = fsm.target.position - transform.position;
                string direction = GetDirection(dirToTarget);
                lastDirection = direction;

                SetActiveSprite(direction);

                if (direction == "Side")
                {
                    bool flip = dirToTarget.x < 0;
                    spriteRenderer.flipX = flip;
                    sideDSpriteRenderer.flipX = flip;
                }

                monAni.PlayWalk(direction);
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
        if (isAttacking) return; // 공격 중이면 스프라이트 표시하지 않음

        // 모두 끄기
        FrontSprite?.SetActive(false);
        BackSprite?.SetActive(false);
        SideSprite?.SetActive(false);

        // 방향별로 선택
        switch (direction)
        {
            case "Front":
                currentSprite = FrontSprite;
                break;
            case "Back":
                currentSprite = BackSprite;
                break;
            case "Side":
                currentSprite = SideSprite;
                break;
        }

        currentSprite?.SetActive(true);
    }


    public abstract void Attack();

    public virtual void TakeDamage(float damage)
    {
        StartCoroutine(DamageAni());
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    IEnumerator DamageAni()
    {
        agent.isStopped = true;

        if (lastDirection == "Front" || lastDirection == "Back" || currentHealth <= 0)
        {
            FrontDSprite.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            FrontDSprite.SetActive(false);
        }
        else
        {
            SideDSprite.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            SideDSprite.SetActive(false);
        }
       
        agent.isStopped = false;
    }


    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    protected virtual void Die()
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent2.enabled = false;
        isDead = true;
        StartCoroutine(DieAni());
    }

    IEnumerator DieAni()
    { 
        FrontSprite.SetActive(false);
        BackSprite.SetActive(false);
        SideSprite.SetActive(false);
        monAni.PlayDie();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SongPeyon"))
        {
            Debug.Log("Monster hit by SongPyeon projectile.");
            if (collision.TryGetComponent<EnemyProjectile>(out EnemyProjectile projectile))
            {
                if (projectile.isReflected)
                {
                    TakeDamage(projectile.damage);
                    Destroy(projectile.gameObject);
                }
            }
            else return;
        }
    }

}
