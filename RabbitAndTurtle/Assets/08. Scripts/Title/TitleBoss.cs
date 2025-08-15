using DG.Tweening;
using UnityEngine;

public class TitleBoss : MonoBehaviour
{
    [SerializeField] float _targetPosition;
    [SerializeField] float _duration;
    [SerializeField] Ease _ease;

    private Tween _tween;

    void Start()
    {
        _tween = transform.DOMoveY(_targetPosition, _duration).SetEase(_ease).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        _tween.Kill();
    }
}
