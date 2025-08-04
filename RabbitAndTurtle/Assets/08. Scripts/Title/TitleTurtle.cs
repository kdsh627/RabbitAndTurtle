using DG.Tweening;
using UnityEngine;

public class TitleTurtle : MonoBehaviour
{
    [SerializeField] float _targetScale;
    [SerializeField] float _duration;
    [SerializeField] Ease _ease;

    private Tween _tween;
    void Start()
    {
        _tween = transform.DOScaleY(_targetScale, _duration).SetEase(_ease).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        _tween.Kill();
    }
}
