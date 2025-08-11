using DG.Tweening;
using UnityEngine;

public class TitleTurtle : MonoBehaviour
{
    [SerializeField] float _targetScale;
    [SerializeField] float _duration;
    [SerializeField] Ease _ease;

    void Start()
    {
        transform.DOScaleY(_targetScale, _duration).SetEase(_ease).SetLoops(-1, LoopType.Yoyo);
    }
}
