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

        Destroy(gameObject);
    }
}
