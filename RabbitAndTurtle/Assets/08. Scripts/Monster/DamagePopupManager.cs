using System.Collections.Generic;
using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    public static DamagePopupManager I { get; private set; }

    [SerializeField] private Canvas canvas;                // 팝업을 올릴 캔버스
    [SerializeField] private DamagePopup popupPrefab;      // 1번에서 만든 프리팹

    readonly Queue<DamagePopup> pool = new();
    RectTransform canvasRT;
    Camera uiCam; // Overlay면 null

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;

        if (!canvas) canvas = GetComponentInParent<Canvas>();
        canvasRT = canvas.transform as RectTransform;
        uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
    }

    public void Show(int amount, Vector3 worldPos, float clampMargin = 32f)
    {
        //Vector2 anchored = WorldToCanvas(worldPos);
        //anchored = ClampToCanvas(anchored, clampMargin);

        //var pop = Get();
        //pop.gameObject.SetActive(true);
        //pop.Play(amount, anchored, () => Return(pop));
    }

    Vector2 WorldToCanvas(Vector3 world)
    {
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(uiCam, world);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, screen, uiCam, out var local);
        return local; // 캔버스 앵커 기준 좌표
    }

    Vector2 ClampToCanvas(Vector2 anchored, float margin)
    {
        var r = canvasRT.rect;
        float x = Mathf.Clamp(anchored.x, r.xMin + margin, r.xMax - margin);
        float y = Mathf.Clamp(anchored.y, r.yMin + margin, r.yMax - margin);
        return new Vector2(x, y);
    }

    DamagePopup Get()
    {
        if (pool.Count > 0) return pool.Dequeue();
        return Instantiate(popupPrefab, canvasRT);
    }

    void Return(DamagePopup p)
    {
        p.gameObject.SetActive(false);
        pool.Enqueue(p);
    }
}
