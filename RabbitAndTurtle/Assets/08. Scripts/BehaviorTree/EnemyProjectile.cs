using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Visual")]
    public Sprite[] SongpeyonSprites;

    [Header("Particle (오른쪽 +X 를 앞머리로 가정)")]
    [SerializeField] private ParticleSystem shootPS; // 자식 파티클

    private MovementRigidbody2D movement;
    private Transform target;
    private Rigidbody2D rb;

    public float damage;
    public bool isReflected;

    private int poolIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 발사체 초기화 (발사 순간)
    /// </summary>
    public void Setup(Transform target, float damage, int poolIndex)
    {
        this.poolIndex = poolIndex;

        // 스프라이트 랜덤 지정
        var sr = GetComponent<SpriteRenderer>();
        if (SongpeyonSprites != null && SongpeyonSprites.Length > 0 && sr != null)
            sr.sprite = SongpeyonSprites[Random.Range(0, SongpeyonSprites.Length)];

        movement = GetComponent<MovementRigidbody2D>();
        this.target = target;
        this.damage = damage;

        // 이동 방향 결정
        Vector2 dir = (target.position - transform.position).normalized;
        movement.MoveTo(dir);

        // 파티클 방향(라디안) 맞추고 터뜨리기
        SetParticleRotation(dir);
        PlayShootParticle();

        isReflected = false;

        // 풀 복귀 예약
        CancelInvoke(nameof(ReturnToPool));
        Invoke(nameof(ReturnToPool), 3f);
    }

    /// <summary>
    /// Block 등으로 반사될 때 호출
    /// </summary>
    private void ReflectNow()
    {
        if (movement == null) return;

        // 이동 반전
        movement.Reflect();

        // 현재 속도 기준으로 새 방향 계산
        Vector2 v = rb ? rb.linearVelocity : Vector2.right;
        if (v.sqrMagnitude < 0.0001f) v = Vector2.right; // 안전장치

        SetParticleRotation(v.normalized);
        PlayShootParticle(); // 반사 스파크를 원치 않으면 이 줄 제거
    }

    private void ReturnToPool()
    {
        CancelInvoke(nameof(ReturnToPool));
        WeaponPool.Instance.Return(poolIndex, gameObject);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(ReturnToPool));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlockCollider"))
        {
            isReflected = true;
            ReflectNow();
        }

        // TODO: 플레이어/벽 등 다른 충돌 처리 필요 시 여기에 추가
    }

    private void SetParticleRotation(Vector2 dir)
    {
        if (shootPS == null) return;

        // 오른쪽(+X)이 0 라디안 기준 → 오프셋 불필요
        float angleRad = Mathf.Atan2(dir.y, dir.x);

        var main = shootPS.main;
        main.startRotation = angleRad;

        // 파티클 오브젝트 자체 회전은 0으로(중복 회전 방지)
        shootPS.transform.rotation = Quaternion.identity;
    }

    private void PlayShootParticle()
    {
        if (shootPS == null) return;
        shootPS.Clear(true);
        shootPS.Play(true);
    }
}
