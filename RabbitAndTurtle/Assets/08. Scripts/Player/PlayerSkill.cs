using System;
using System.Collections;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject WhalePrefab;
    public float spawnYOffset = 1f;

    [SerializeField] private int _level = 1;
    private int _maxLevel = 5;

    [Header("---- 쿨타임 ----")]
    [SerializeField] private float _maxCoolTime = 10f;
    [SerializeField] private float _coolTime = 0f;
    [SerializeField] private bool isCoolTime = false;

    public float CoolTime => _coolTime;
    public float MaxCoolTime => _maxCoolTime;

    [Header("연출")]
    public float spawnDelay = 0.5f;   // 스폰 애니 길이(클립 길이에 맞춰 조절)

    private PlayerMovement playerMovement;

    public event Action OnCoolTimeChanged;
    public event Action OnSkillActive;

    void Start()
    {
        OnCoolTimeChanged?.Invoke();
        OnSkillActive?.Invoke();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //    QSkill();
        if (isCoolTime)
        {
            QskillCoolTime();
        }
    }

    private void QskillCoolTime()
    {
        if (_coolTime > float.Epsilon)
        {
            _coolTime -= Time.deltaTime;

            //여기서 쿨타임 이벤트
            OnCoolTimeChanged?.Invoke();
        }
        else
        {
            //쿨타임 다 돌면
            isCoolTime = false;
            _coolTime = 0f;

            //여기서 활성화 이펙트
            OnSkillActive?.Invoke();
        }
    }

    public void Levelup()
    {
        _level++;
    }

    public bool isMaxLevel()
    {
        return _level == _maxLevel;
    }


    public void QSkill()
    {
        if (isCoolTime) return;

        isCoolTime = true;
        _coolTime = _maxCoolTime;

        string dir = playerMovement.lastDirection;
        bool isLeft = GetComponent<SpriteRenderer>().flipX;

        Vector2 throwDir = GetThrowDirection(dir, isLeft);
        StartCoroutine(WhaleThrow(throwDir, isLeft, dir, _level));
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

    IEnumerator WhaleThrow(Vector2 throwDir, bool isLeft, string rawDir, int level)
    {
        // 플레이어 잠깐 멈춤 (원하면 제거)
        float prevSpeed = playerMovement.moveSpeed;
        playerMovement.moveSpeed = 0f;

        Vector3 spawnPos = transform.position;
        spawnPos.y += spawnYOffset;

        GameObject whale = Instantiate(WhalePrefab, spawnPos, Quaternion.identity);

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

        // 레벨 조정
        proj.LevelMultiply(level);

        // 발사
        proj.Launch(throwDir, mode);
    }

    void WSkill() => StartCoroutine(FlyFishShot());
    void ESkill() => StartCoroutine(TurtleShield());

    IEnumerator FlyFishShot() { yield return null; }
    IEnumerator TurtleShield() { yield return null; }
}
