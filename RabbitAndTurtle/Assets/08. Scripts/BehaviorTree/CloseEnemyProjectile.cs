using System.Collections;
using UnityEngine;

public class CloseEnemyProjectile : MonoBehaviour
{
    private Transform target;
    private Transform followOrigin; // 몬스터 무기 소환 위치
    public float damage;
    public string dir;
    private Vector3 offset; // 회전 적용된 초기 위치 보정값
    private Animator ani;
    private EnemyFSM fsm;
    public bool isHit;
    private BaseMonster ownerMonster; // 발사 주체
    private int poolIndex;
    public string DirState;


    private void Awake()
    {
        fsm = GetComponent<EnemyFSM>();
        ani = GetComponent<Animator>();
        isHit = false;
    }

    public void Setup(Transform target, float damage, Transform followOrigin, string dir, BaseMonster owner, int poolIndex)
    {
        this.target = target;
        this.damage = damage;
        this.followOrigin = followOrigin;
        this.dir = dir;
        this.ownerMonster = owner;
        this.poolIndex = poolIndex;

        offset = transform.position - followOrigin.position;

        PlayAnimation();
        Invoke(nameof(ReturnToPool), 0.8f);
    }

    private void PlayAnimation()
    {
        switch (dir)
        {
            case "Front":
                {
                    ani.Play("BaataSwingFront"); 
                    break;
                }
            case "Back":
                { 
                    ani.Play("BaataSwingBack"); break; 
                }
            case "Left":
                {
                    ani.Play("BaataSwingLeft"); break;
                }
            case "Right":
                {
                    ani.Play("BaataSwingRight"); break;
                }
        }
        StartCoroutine(HitSoundDelay());
    }

    private void Update()
    {
        if (followOrigin == null)
        {
            ReturnToPool();
            return;
        }

        transform.position = followOrigin.position + offset;
    }

    private void ReturnToPool()
    {
        WeaponPool.Instance.Return(poolIndex, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlockCollider") && !isHit)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.ShieldSuccess);
            isHit = true;
            if (ownerMonster != null)
                ownerMonster.TakeDamage(damage);

            ReturnToPool();
        }
    }

    protected string GetAttackDirection(Vector3 toTarget)
    {
        if (Mathf.Abs(toTarget.x) > Mathf.Abs(toTarget.y))
            return toTarget.x > 0 ? "Right" : "Left";
        else
            return toTarget.y > 0 ? "Back" : "Front";
    }

    IEnumerator HitSoundDelay()
    {
        yield return new WaitForSeconds(0.35f);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.CloseSwing);
    }
}
