using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAnimationController animatorController;
    private PlayerBlock playerBlock; // PlayerBlock 컴포넌트 추가
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer sideDSpriteRenderer;
    private PlayerStat playerStat; // PlayerStat 컴포넌트 추가
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    public string lastDirection = "Front"; // 마지막 방향 저장용

    public GameObject FrontDSprite;
    public GameObject SideDSprite;
    public GameObject WaterEffect;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<PlayerAnimationController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sideDSpriteRenderer = SideDSprite.GetComponent<SpriteRenderer>();
        playerBlock = GetComponent<PlayerBlock>();
        playerStat = GetComponent<PlayerStat>(); // PlayerStat 컴포넌트 초기화
    }
    private void Start()
    {
        FrontDSprite.SetActive(false);
        SideDSprite.SetActive(false);
        WaterEffect.SetActive(false);
    }
    public void SetMoveInput(Vector2 input)
    {
        // 죽었으면 어떤 입력/회전/애니메이션 갱신도 하지 않음
        if (playerStat != null && playerStat.isDie)
        {
            moveInput = Vector2.zero;          // 혹시 남아있는 이동 입력 제거
            return;
        }

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
                bool flip = moveInput.x < 0;
                spriteRenderer.flipX = flip;
                sideDSpriteRenderer.flipX = flip;
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
        if(playerStat.isDie) return;

        if (playerBlock.isBlock || playerBlock.isExhausted)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            WaterEffect.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            WaterEffect.SetActive(false);
        }
    }
}
