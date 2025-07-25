using System.Collections;
using UnityEngine;

public class CloseEnemyProjectile : MonoBehaviour
{
    private Transform target;
    private Transform followOrigin; // 몬스터 무기 소환 위치
    public float damage;
    string dir;
    private Vector3 offset; // 회전 적용된 초기 위치 보정값
    private Animator ani;
    private EnemyFSM fsm;
    public bool isHit;
    private BaseMonster ownerMonster; // 발사 주체


    private void Awake()
    {
        fsm = GetComponent<EnemyFSM>();
        ani = GetComponent<Animator>();
        isHit = false;
    }

    public void Setup(Transform target, float damage, Transform followOrigin, string dir, BaseMonster owner)
    {
        this.target = target;
        this.damage = damage;
        this.followOrigin = followOrigin;
        this.dir = dir;
        this.ownerMonster = owner;

        offset = transform.position - followOrigin.position;
    }


    private void Update()
    {
        if (followOrigin == null)
        {
            Destroy(gameObject);
            return;
        }

        // 부모 위치 + 보정값으로 위치만 따라감
        transform.position = followOrigin.position + offset;
    }

    private void Start()
    {
        switch (dir)
        {
            case "Front":
                ani.Play("BaataSwingFront");
                break;
            case "Back":
                ani.Play("BaataSwingBack");
                break;
            case "Left":
                ani.Play("BaataSwingLeft");
                break;
            case "Right":
                ani.Play("BaataSwingRight");
                break;
        }

        Destroy(gameObject, 1f); // 1초 후 삭제
    }

    protected string GetAttackDirection(Vector3 toTarget)
    {
        if (Mathf.Abs(toTarget.x) > Mathf.Abs(toTarget.y))
            return toTarget.x > 0 ? "Right" : "Left";
        else
            return toTarget.y > 0 ? "Back" : "Front";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlockCollider"))
        {
            if (ownerMonster != null && !isHit)
            {
                isHit = true; // 중복 데미지 방지
                ownerMonster.TakeDamage(damage); // 주체에게 피해 줌
                Debug.Log($"{ownerMonster.name}이 자신이 던진 무기에 맞아 {damage} 피해!");
            }

            Destroy(gameObject);
        }
    }

}
