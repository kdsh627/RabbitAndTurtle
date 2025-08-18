using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BugWander2D : MonoBehaviour
{
    [Header("Move Area (둘 중 하나 사용)")]
    public BoxCollider2D areaCollider;          // 이걸 넣으면 우선 사용
    public Vector2 areaCenter = Vector2.zero;   // 직접 지정용
    public Vector2 areaSize = new Vector2(10, 6);

    [Header("Speed")]
    public float baseSpeed = 2.5f;
    public float speedVariance = 1.0f;          // 속도 랜덤 가감
    public float turnLerp = 8f;                 // 회전(방향 전환) 민감도

    [Header("Wander Target")]
    public float targetRadius = 0.5f;           // 목표에 도달했다고 치는 거리
    public Vector2 retargetTimeRange = new Vector2(1.5f, 3.0f);  // 목표 갱신 간격

    [Header("Buggy Feel (흔들림)")]
    public float noiseFreq = 2.0f;              // 페를린 노이즈 주파수
    public float noiseAngleAmplitude = 35f;     // 방향 흔들림 각도(도)
    public float microJitter = 0.25f;           // 아주 미세한 좌우 떨림 세기

    [Header("Edge Avoidance (가장자리 회피)")]
    public float edgePadding = 0.6f;            // 경계에서 이만큼 남기고 밀어냄
    public float edgePush = 6f;                  // 경계에서 중앙으로 미는 힘

    [Header("Burst (가끔 번쩍 가속)")]
    [Range(0, 1)] public float burstChancePerRetarget = 0.3f;
    public float burstMultiplier = 1.9f;        // 번쩍일 때 속도 곱
    public float burstDuration = 0.25f;

    [Header("Facing")]
    public bool rotateZToFaceDir = false;       // 스프라이트를 진행방향으로 회전할지

    Rigidbody2D rb;
    Vector2 currentDir = Vector2.right;
    Vector2 targetPos;
    float nextRetargetAt;
    float seed;
    float currentSpeedMul = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        seed = Random.value * 1000f;
        PickNewTarget(true);
    }

    void FixedUpdate()
    {
        // 1) 현재 영역/경계 계산
        var (center, size) = GetArea();
        var half = size * 0.5f;

        // 2) 타겟 갱신 타이밍 or 타겟 도착 시 새 타겟
        if (Time.time >= nextRetargetAt || (targetPos - (Vector2)transform.position).sqrMagnitude <= targetRadius * targetRadius)
        {
            PickNewTarget(false);
        }

        // 3) 기본 목표 방향
        Vector2 toTarget = (targetPos - (Vector2)transform.position);
        Vector2 desiredDir = toTarget.sqrMagnitude > 0.0001f ? toTarget.normalized : currentDir;

        // 4) 페를린 노이즈로 방향 흔들림(부드러운 벌레 느낌)
        float t = Time.time * noiseFreq;
        float n = Mathf.PerlinNoise(seed, t) * 2f - 1f; // [-1,1]
        float angle = n * noiseAngleAmplitude;
        desiredDir = Rotate(desiredDir, angle);

        // 5) 마이크로 지터(살짝 좌우 떨림)
        float j = (Mathf.PerlinNoise(seed + 77.7f, t * 1.7f) * 2f - 1f) * microJitter;
        desiredDir = (desiredDir + new Vector2(-desiredDir.y, desiredDir.x) * j).normalized;

        // 6) 경계 가까우면 중앙으로 밀어주기
        Vector2 local = (Vector2)transform.position - center;
        Vector2 push = Vector2.zero;
        if (Mathf.Abs(local.x) > half.x - edgePadding)
            push.x = -Mathf.Sign(local.x);
        if (Mathf.Abs(local.y) > half.y - edgePadding)
            push.y = -Mathf.Sign(local.y);

        if (push.sqrMagnitude > 0f)
        {
            desiredDir = Vector2.Lerp(desiredDir, (desiredDir + push.normalized * edgePush).normalized, 0.6f);
        }

        // 7) 속도 계산
        float targetSpeed = (baseSpeed + RandomSigned() * speedVariance * 0.15f) * currentSpeedMul; // 프레임마다 약간의 미세 랜덤
        Vector2 desiredVel = desiredDir * targetSpeed;

        // 8) 현재 진행 방향을 서서히 desired로 보간 (벌레가 급격히 꺾이지 않게)
        Vector2 newDir = Vector2.Lerp(currentDir, desiredDir, turnLerp * Time.fixedDeltaTime).normalized;
        currentDir = newDir;

        rb.linearVelocity = newDir * targetSpeed;

        if (rotateZToFaceDir && rb.linearVelocity.sqrMagnitude > 0.0001f)
        {
            float z = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            rb.MoveRotation(z);
        }
    }

    void PickNewTarget(bool first)
    {
        var (center, size) = GetArea();
        var half = size * 0.5f;

        // 영역 안에서 랜덤 포인트
        targetPos = center + new Vector2(
            Random.Range(-half.x + edgePadding, half.x - edgePadding),
            Random.Range(-half.y + edgePadding, half.y - edgePadding)
        );

        // 다음 타이밍
        float dt = Random.Range(retargetTimeRange.x, retargetTimeRange.y);
        nextRetargetAt = Time.time + dt;

        // 가끔 번쩍 가속
        if (!first && Random.value < burstChancePerRetarget)
        {
            StopAllCoroutines();
            StartCoroutine(BurstCo());
        }
    }

    System.Collections.IEnumerator BurstCo()
    {
        currentSpeedMul = burstMultiplier;
        yield return new WaitForSeconds(burstDuration);
        currentSpeedMul = 1f;
    }

    (Vector2 center, Vector2 size) GetArea()
    {
        if (areaCollider != null)
        {
            var b = areaCollider.bounds;
            return ((Vector2)b.center, (Vector2)b.size);
        }
        return (areaCenter, areaSize);
    }

    static Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float c = Mathf.Cos(rad);
        float s = Mathf.Sin(rad);
        return new Vector2(c * v.x - s * v.y, s * v.x + c * v.y);
    }

    static float RandomSigned() => Random.value * 2f - 1f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var (c, s) = GetArea();
        Gizmos.DrawWireCube(c, s);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPos, 0.1f);
    }
}
