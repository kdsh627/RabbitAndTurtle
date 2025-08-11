using DG.Tweening;
using UnityEngine;

public class TitleBoss : MonoBehaviour
{
    [SerializeField] float _targetPosition;
    [SerializeField] float _duration;
    [SerializeField] Ease _ease;

    void Start()
    {
        transform.DOMoveY(_targetPosition, _duration).SetEase(_ease).SetLoops(-1, LoopType.Yoyo);
    }
}
