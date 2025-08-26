using System.Collections;
using System.ComponentModel;
using UnityEngine;

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

    [SerializeField] private float knockbackForce = 8f; // 세기
    [SerializeField] private float knockbackTime = 0.12f; 
    private Coroutine knockbackCo;
    private bool isKnockbacking = false;

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
        if (playerStat.isDie) return;
        if (isKnockbacking) return;

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

    public void KnockBack(string dir)
    {
        KnockBack(dir, knockbackForce);
    }

    public void KnockBack(string dir, float force)
    {
        if (playerStat != null && playerStat.isDie) return;
        if (rb == null) return;

        Vector2 v = DirStringToVector(dir);
        if (v == Vector2.zero) return;

        if (knockbackCo != null) StopCoroutine(knockbackCo);
        knockbackCo = StartCoroutine(KnockbackRoutineByDir(v, force));
    }

    // 문자열 → 방향 벡터
    private Vector2 DirStringToVector(string dir)
    {
        if (string.IsNullOrEmpty(dir)) return Vector2.zero;
        var s = dir.Trim().ToLowerInvariant();

        if (s == "front" || s == "down") return Vector2.down;  
        if (s == "back" || s == "up") return Vector2.up;    
        if (s == "left") return Vector2.left;
        if (s == "right") return Vector2.right;

        return Vector2.zero;
    }

    // 방향 기반 넉백 코루틴
    private IEnumerator KnockbackRoutineByDir(Vector2 dir, float force)
    {
        isKnockbacking = true;                  // 입력 이동 잠깐 중지(원하면 주석 처리)
        rb.linearVelocity = Vector2.zero;       // 잔여 속도 제거
        rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);

        float t = 0f;
        while (t < knockbackTime)
        {
            t += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;       // 정지
        isKnockbacking = false;
        knockbackCo = null;
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
