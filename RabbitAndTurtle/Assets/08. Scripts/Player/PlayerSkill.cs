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
        Debug.Log("Q 스킬 사용"); 
        StartCoroutine(WhaleThrow());
    }

    void WSkill()
    {
        StartCoroutine(FlyFishShot());
    }

    void ESkill()
    {
        StartCoroutine(TurtleShield());
    }

    IEnumerator WhaleThrow()
    {
        float speed =  playerMovement.moveSpeed; // 이동 멈추기
        playerMovement.moveSpeed = 0f;
        // 1. 던질 방향 계산
        string dir = playerMovement.lastDirection;
        Vector2 throwDir = Vector2.zero;

        switch (dir)
        {
            case "Front":
                throwDir = Vector2.down;
                break;
            case "Back":
                throwDir = Vector2.up;
                break;
            case "Side":
                // flipX 기준 왼쪽/오른쪽 결정
                bool isLeft = playerMovement.GetComponent<SpriteRenderer>().flipX;
                throwDir = isLeft ? Vector2.left : Vector2.right;
                break;
        }

        // 2. Instantiate + 던지기
        GameObject whale = Instantiate(WhalePrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        playerMovement.moveSpeed = speed; // 이동 속도 복원
        Rigidbody2D rb = whale.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = throwDir * throwPower;
        }
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
