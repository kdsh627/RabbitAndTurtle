using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 localPoint;

        // 마우스의 스크린 좌표를 캔버스의 로컬 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rect.parent as RectTransform,
            Input.mousePosition,
            Camera.main,
            out localPoint);

        _rect.localPosition = localPoint;
    }
}
