using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAnimationController animatorController;
    private PlayerBlock playerBlock; // PlayerBlock 컴포넌트 추가
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer sideDSpriteRenderer;
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    public string lastDirection = "Front"; // 마지막 방향 저장용

    public GameObject FrontDSprite;
    public GameObject SideDSprite;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<PlayerAnimationController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sideDSpriteRenderer = SideDSprite.GetComponent<SpriteRenderer>();
        playerBlock = GetComponent<PlayerBlock>();
    }
    private void Start()
    {
        FrontDSprite.SetActive(false);
        SideDSprite.SetActive(false);
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
                sideDSpriteRenderer.flipX = moveInput.x < 0;
            }

            animatorController.PlayWalk(direction);
        }
    }

    public IEnumerator DamageAni()
    {
        if (lastDirection == "Front" || lastDirection == "Back")
        {
            FrontDSprite.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            FrontDSprite.SetActive(false);
        }
        else
        {
            SideDSprite.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            SideDSprite.SetActive(false);
        }
    }
    void FixedUpdate()
    {
        if(playerBlock.isBlock || playerBlock.isExhausted)
            rb.MovePosition(rb.position + moveInput.normalized * (moveSpeed - 4f) * Time.fixedDeltaTime);

        else
            rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }   

    private string GetDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) return "Side";
        else if (dir.y > 0) return "Back";
        else return "Front";
    }
}
