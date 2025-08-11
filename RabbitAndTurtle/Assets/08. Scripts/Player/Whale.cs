using System.Collections;
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

                // 0→1→0 형태의 간단 곡선(수학식)
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
        Destroy(gameObject);
    }
}
