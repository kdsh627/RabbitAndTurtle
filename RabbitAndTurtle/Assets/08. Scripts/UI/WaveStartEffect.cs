using DG.Tweening;
using UnityEngine;

public class WaveStartEffect : MonoBehaviour
{
    [Header("---- 효과 관련 속성 ----")]
    [SerializeField] private GameObject _waveText;
    [SerializeField] private Vector2 _startPostion;
    [SerializeField] private int _firstEndValue;
    [SerializeField] private int _secondEndValue;

    private Sequence sequence;
    private void OnEnable()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = _startPostion;

        sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOAnchorPosX(_firstEndValue, 1.0f).SetEase(Ease.OutCubic))
            .Append(rectTransform.DOAnchorPosX(_secondEndValue, 1.0f).SetEase(Ease.InCubic))
            .OnComplete(() => { _waveText.SetActive(false); });

    }
    private void OnDisable()
    {
        sequence.Kill();
    }
}
