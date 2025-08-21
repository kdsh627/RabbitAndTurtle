using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;

public class Drop : MonoBehaviour
{
    private Tween _tween;
    private SpriteRenderer _spriteRenderer;
    private bool _isPlaying = false;
    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        _tween.Kill();
    }

    public void GetItem()
    {
        if (_isPlaying) return;
        _isPlaying = true;

        _tween = _spriteRenderer.DOFade(0, 1f).OnComplete(() => { Destroy(gameObject); });
    }
}
