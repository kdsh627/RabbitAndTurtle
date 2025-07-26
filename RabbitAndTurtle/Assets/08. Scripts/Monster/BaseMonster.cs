using NUnit.Framework.Constraints;
using System.Collections;
using Unity.Behavior.Demo;
using Unity.Cinemachine;
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
   
    private GameObject currentSprite;
    private SpriteRenderer spriteRenderer;
    private MonsterAnimationController monAni;
    public float moveThreshold = 0.01f;

    private Vector3 lastPosition;
    private string lastDirection = "Front";

    private NavMeshAgent agent;
    private EnemyFSM fsm;
    private bool isAttacking = false;

    protected virtual void Start()
    { 
        monAni = GetComponent<MonsterAnimationController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        fsm = GetComponent<EnemyFSM>();

        currentHealth = MonsterHealth;
        lastPosition = transform.position;
    }

    protected virtual void Update()
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
                spriteRenderer.flipX = dirToTarget.x < 0;

            monAni.PlayWalk(direction);
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
        yield return new WaitForSeconds(1f);
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
        monAni.PlayHurt();
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    protected virtual void Die()
    {
        Debug.Log("Monster has died.");
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
