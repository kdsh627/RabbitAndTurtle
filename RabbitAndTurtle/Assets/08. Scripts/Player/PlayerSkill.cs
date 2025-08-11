using System.Collections;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject WhalePrefab;

    [Header("연출")]
    public float spawnDelay = 0.5f;   // 스폰 애니 길이(클립 길이에 맞춰 조절)

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            QSkill();
    }

    public void QSkill()
    {
        string dir = playerMovement.lastDirection;
        bool isLeft = GetComponent<SpriteRenderer>().flipX;

        Vector2 throwDir = GetThrowDirection(dir, isLeft);
        StartCoroutine(WhaleThrow(throwDir, isLeft, dir));
    }

    private Vector2 GetThrowDirection(string dir, bool isLeft)
    {
        switch (dir)
        {
            case "Front": return Vector2.down;                 // 아래
            case "Back": return Vector2.up;                   // 위
            case "Side": return isLeft ? Vector2.left : Vector2.right; // 좌/우
            default: return Vector2.zero;
        }
    }

    IEnumerator WhaleThrow(Vector2 throwDir, bool isLeft, string rawDir)
    {
        // 플레이어 잠깐 멈춤 (원하면 제거)
        float prevSpeed = playerMovement.moveSpeed;
        playerMovement.moveSpeed = 0f;

        GameObject whale = Instantiate(WhalePrefab, transform.position, Quaternion.identity);

        // 스폰 애니 있으면 재생
        Animator anim = whale.GetComponent<Animator>();
        if (anim != null && spawnDelay > 0f)
        {
            anim.Play(isLeft ? "WhaleSpawnLeft" : "WhaleSpawnRight");
            yield return new WaitForSeconds(spawnDelay);
        }

        // 이동 재개
        playerMovement.moveSpeed = prevSpeed;

     
        var proj = whale.GetComponent<Whale>(); // 또는 Whale

        // 좌/우 = 포물선, 위/아래 = 스케일
        ThrowMode mode = (rawDir == "Side") ? ThrowMode.SideParabola : ThrowMode.VerticalScale;

        // 발사
        proj.Launch(throwDir, mode);
    }

    void WSkill() => StartCoroutine(FlyFishShot());
    void ESkill() => StartCoroutine(TurtleShield());

    IEnumerator FlyFishShot() { yield return null; }
    IEnumerator TurtleShield() { yield return null; }
}
