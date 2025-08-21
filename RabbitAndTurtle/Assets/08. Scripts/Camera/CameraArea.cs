using Unity.Cinemachine;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    [Header("--- 카메라 영역 ---")]
    [SerializeField] private PolygonCollider2D _confinerCollider;

    [Header("--- 카메라 ---")]
    [SerializeField] private CinemachineBrain _camera;

    private void Awake()
    {
        _camera = Camera.main.GetComponent<CinemachineBrain>();

        CinemachineConfiner2D cinemachineConfiner2D = (_camera.ActiveVirtualCamera as CinemachineCamera).GetComponent<CinemachineConfiner2D>();
        _confinerCollider = GetComponent<PolygonCollider2D>();
        cinemachineConfiner2D.BoundingShape2D = _confinerCollider;
        cinemachineConfiner2D.InvalidateBoundingShapeCache();
    }
}
