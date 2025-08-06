using System.Collections;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject WhalePrefab;
    public float throwPower = 10f; // 던지는 힘 조절 변수

    private PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QSkill();
        }
    }


    public void QSkill()
    {
        string dir = playerMovement.lastDirection;
        bool isLeft = playerMovement.GetComponent<SpriteRenderer>().flipX;

        Vector2 throwDir = GetThrowDirection(dir, isLeft);

        // 이 시점의 방향 상태를 모두 고정해서 넘겨줌!
        StartCoroutine(WhaleThrow(throwDir, isLeft));
    }

    private Vector2 GetThrowDirection(string dir, bool isLeft)
    {
        switch (dir)
        {
            case "Front": return Vector2.down;
            case "Back": return Vector2.up;
            case "Side": return isLeft ? Vector2.left : Vector2.right;
            default: return Vector2.zero;
        }
    }

    // flipX까지 매개변수로 받음
    IEnumerator WhaleThrow(Vector2 throwDir, bool isLeft)
    {
        float speed = playerMovement.moveSpeed;
        playerMovement.moveSpeed = 0f;

        GameObject whale = Instantiate(WhalePrefab, transform.position, Quaternion.identity);
        Animator whaleAnimator = whale.GetComponent<Animator>();
        if(isLeft) whaleAnimator.Play("WhaleSpawnLeft");
        else whaleAnimator.Play("WhaleSpawnRight");
        yield return new WaitForSeconds(0.5f);

        playerMovement.moveSpeed = speed;

        Rigidbody2D rb = whale.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = throwDir * throwPower;
        }

    
    }


    void WSkill()
    {
        StartCoroutine(FlyFishShot());
    }

    void ESkill()
    {
        StartCoroutine(TurtleShield());
    }


    IEnumerator FlyFishShot()
    {
        yield return null;
        //스킬 추가 예정
    }

    IEnumerator TurtleShield()
    {
        yield return null;
        //스킬추가 예정
    }
}
