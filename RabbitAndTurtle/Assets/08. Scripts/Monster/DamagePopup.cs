using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup cg;

    [Header("Anim")]
    public float spawnSpreadX = 24f;   // 시작 위치의 가로 랜덤
    public float spawnSpreadY = 12f;   // 시작 위치의 세로 랜덤
    public Vector2 upYRange = new Vector2(60f, 100f); // 얼마나 위로 올라갈지
    public float life = 0.6f;          // 전체 재생시간

    RectTransform rt;
    Sequence seq;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        if (!cg) cg = GetComponent<CanvasGroup>();
    }

    void OnDisable() { seq?.Kill(); }

    public void Play(int amount, Vector2 anchoredStart, System.Action onComplete)
    {
        text.text = amount.ToString(); // 필요하면 ToString("N0")

        // 시작 위치(앵커 기준) + 살짝 랜덤
        Vector2 start = anchoredStart + new Vector2(
            Random.Range(-spawnSpreadX, spawnSpreadX),
            Random.Range(0f, spawnSpreadY)
        );
        rt.anchoredPosition = start;

        // 올라갈 목표 위치(약간 좌우 랜덤)
        Vector2 end = start + new Vector2(Random.Range(-10f, 10f),
                                          Random.Range(upYRange.x, upYRange.y));

        // 초기 상태
        seq?.Kill();
        cg.alpha = 0f;
        rt.localScale = Vector3.one * 0.9f;

        // 애니
        seq = DOTween.Sequence().SetUpdate(false) // timeScale 영향 받도록(일시정지 시 멈춤)
            .Append(cg.DOFade(1f, 0.06f))
            .Join(rt.DOScale(1.15f, 0.12f).SetEase(Ease.OutBack, 2f))
            .Append(rt.DOAnchorPos(end, life * 0.7f).SetEase(Ease.OutCubic))
            .Join(cg.DOFade(0f, life * 0.45f).SetDelay(life * 0.25f))
            .OnComplete(() => onComplete?.Invoke());
    }
}
