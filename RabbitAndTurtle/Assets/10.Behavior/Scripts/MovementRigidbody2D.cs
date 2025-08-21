using UnityEngine;

public class MovementRigidbody2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Rigidbody2D rigid2D;

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    public void MoveTo(Vector3 direction)
    {
        rigid2D.linearVelocity = direction * moveSpeed;
    }

    public void Reflect()
    {
        if (rigid2D != null)
        {
            rigid2D.linearVelocity *= -1f; // 현재 방향의 정반대로 반사
        }
    }

}

