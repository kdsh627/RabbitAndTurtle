using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerAnimationController animatorController;
    public SpriteRenderer spriteRenderer;
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private string lastDirection = "Front"; // 마지막 방향 저장용

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<PlayerAnimationController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;

        if (moveInput == Vector2.zero)
        {
            animatorController.PlayIdle(lastDirection);
        }
        else
        {
            string direction = GetDirection(moveInput);
            lastDirection = direction;

            if (direction == "Side")
            {
                // 오른쪽 기준 → 왼쪽이면 flipX
                spriteRenderer.flipX = moveInput.x < 0;
            }

            animatorController.PlayWalk(direction);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private string GetDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) return "Side";
        else if (dir.y > 0) return "Back";
        else return "Front";
    }
}
