using UnityEngine;

public class CloseEnemyProjectile : MonoBehaviour
{
    private Transform target;
    private Transform followOrigin; // 몬스터 무기 소환 위치
    private float damage;
    string dir;
    private Vector3 offset; // 회전 적용된 초기 위치 보정값
    private Animator ani;
    private EnemyFSM fsm;

    private void Awake()
    {
        fsm = GetComponent<EnemyFSM>();
        ani = GetComponent<Animator>();
    }

    public void Setup(Transform target, float damage, Transform followOrigin, string dir)
    {
        this.target = target;
        this.damage = damage;
        this.followOrigin = followOrigin;
        this.dir = dir;

        // offset 계산 (현재 위치에서 부모까지의 상대 벡터)
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
}
