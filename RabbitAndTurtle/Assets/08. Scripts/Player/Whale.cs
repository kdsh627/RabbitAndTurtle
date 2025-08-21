using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public enum ThrowMode { SideParabola, VerticalScale }

public class Whale : MonoBehaviour
{
    public float range = 6f;
    public float flightTime = 0.8f;
    public float arcHeight = 1.5f;   // 포물선 높이
    public float scaleAmount = 0.25f; // 커졌다 작아질 정도
    public GameObject landEffectPrefab;
    public float effectLife = 1.2f;
    public float peakScaleMultiplier = 1.3f;

    [Header("던질 때 켤 파티클")]
    public ParticleSystem Effect;     // 처음엔 꺼둔 상태(비활성 or Stop)

    [Header("카메라 흔들림")]
    public CinemachineImpulseSource _impulseSource;

    Vector3 baseScale;
    bool running;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    public void Launch(Vector2 dir, ThrowMode mode)
    {
        if (running) return;
        StartCoroutine(DoLaunch(dir.normalized, mode));
    }

    IEnumerator DoLaunch(Vector2 dir, ThrowMode mode)
    {
        running = true;

        // ▼ 던지기 시작 시 파티클 켜기 + 오른쪽이면 flipX
        PlayParticleForDir(dir);

        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)(dir * range);
        Vector2 offsetDir = Vector2.up; // 포물선 위쪽 방향

        float t = 0f;
        while (t < flightTime)
        {
            float n = Mathf.Clamp01(t / flightTime);
            Vector3 pos = Vector3.Lerp(start, end, n);

            if (mode == ThrowMode.SideParabola)
            {
                // 포물선 높이
                float h = 4f * arcHeight * n * (1f - n);
                pos += (Vector3)(offsetDir * h);
                transform.position = pos;
            }
            else // VerticalScale
            {
                transform.position = pos;

                // 0→1→0 형태
                float k = 1f - Mathf.Abs(2f * n - 1f); // 0,1,0
                float s = Mathf.Lerp(1f, peakScaleMultiplier, k);
                transform.localScale = baseScale * s;
            }

            t += Time.deltaTime;
            yield return null;
        }

        // 착지
        transform.position = end;
        transform.localScale = baseScale;

        if (landEffectPrefab)
        {
            var eff = Instantiate(landEffectPrefab, end, Quaternion.identity);
            Destroy(eff, effectLife);
        }

        _impulseSource.GenerateImpulse();
        Destroy(gameObject);
    }

    void PlayParticleForDir(Vector2 dir)
    {
        if (Effect == null) return;

        // Renderer 가져오기
        var psr = Effect.GetComponent<ParticleSystemRenderer>();
        if (psr != null)
        {
            // 왼쪽으로 던질 땐 그대로(0), 오른쪽일 때만 flipX(= U축 뒤집기)
            psr.flip = (dir.x > 0f) ? Vector3.zero : new Vector3(1f, 0f, 0f);
        }

        if (!Effect.gameObject.activeSelf) Effect.gameObject.SetActive(true);
        Effect.Clear(true);
        Effect.Play(true);
    }
}
