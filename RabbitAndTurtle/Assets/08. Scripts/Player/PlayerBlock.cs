using System;
using Manager;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAnimationController animatorController;

    [Header("방어 변수")]
    public float MaxBlockTime = 0.5f;           // 최대 방어 시간
    public float ExhaustThreshold = 0.4f;       // 빨간선 임계값
    public float RecoveryRate = 0.5f;           // Idle 회복 속도
    public float ExhaustedDelay = 1f;           // 탈진 시 회복 지연 시간
    private float blockCooldown = 0.1f; // 방어 최소 간격
    private float lastBlockTime = -999f; // 마지막 방어 시도 시간 기록



    public GameObject ExhaustedEff;
    public GameObject BlockCollider;

    public bool isBlock { get; private set; } = false;
    public bool isExhausted { get; private set; } = false;

    public event Action ValueChanged;

    private enum BlockState
    {
        Idle,
        Blocking,
        Exhausted
    }

    private BlockState currentState = BlockState.Idle;

    private float currentGauge;
    public float CurrentGauge
    {
        get => currentGauge;
        set
        {
            currentGauge = value;
            ValueChanged?.Invoke();
        }
    }

    private float exhaustedTimer = 0f; // 탈진 대기 타이머

    private InputManager inputManager;

    private bool isBlockButtonHeld = false;

    private PlayerStat playerStat;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animatorController = GetComponent<PlayerAnimationController>();
        playerStat = GetComponent<PlayerStat>(); // PlayerStat 컴포넌트 초기화 
        CurrentGauge = MaxBlockTime;
        ExhaustedEff.SetActive(false); // 탈진 이펙트 비활성화
        BlockCollider.SetActive(false);
    }

    void Update()
    {
        if (!playerStat.isDie)
        {
            switch (currentState)
            {
                case BlockState.Idle:
                    HandleIdle();
                    break;

                case BlockState.Blocking:
                    HandleBlocking();
                    break;

                case BlockState.Exhausted:
                    HandleExhausted();
                    break;
            }
        }
    }

    public void SetBlocking(bool isHeld)
    {
        if (isHeld)
        {
            if (Time.time - lastBlockTime < blockCooldown)
                return;

            lastBlockTime = Time.time;
        }

        isBlockButtonHeld = isHeld;
    }


    private void HandleIdle()
    {
        if (isBlockButtonHeld && CurrentGauge > ExhaustThreshold)
        {
            StartBlocking();
        }
        else
        {
            RecoverGauge(RecoveryRate, MaxBlockTime);
        }
    }

    private void HandleBlocking()
    {
        if (!BlockCollider.activeSelf)
            BlockCollider.SetActive(true);

        CurrentGauge -= Time.deltaTime;
        CurrentGauge = Mathf.Max(CurrentGauge, 0f);

        if (!isBlockButtonHeld)
        {
            StopBlocking(); // 그냥 Idle로 전환
            BlockCollider.SetActive(false);
            return;
        }

        if (CurrentGauge <= ExhaustThreshold)
        {
            EnterExhausted(); // 상태 전환만 수행
            BlockCollider.SetActive(false);
            return;
        }
    }



    private void HandleExhausted()
    {
        if (!ExhaustedEff.activeSelf)
            ExhaustedEff.SetActive(true); // 탈진 시작 시 이펙트 켜기

        if (exhaustedTimer < ExhaustedDelay)
        {
            exhaustedTimer += Time.deltaTime;
            return; // 대기 중에는 회복 안 함
        }

        // ExhaustThreshold까지 회복
        RecoverGauge(RecoveryRate, ExhaustThreshold);

        if (CurrentGauge >= ExhaustThreshold)
        {
            currentState = BlockState.Idle;
            ExhaustedEff.SetActive(false); // 회복 완료 후 이펙트 끄기
            isExhausted = false; // 탈진 상태로 설정
        }
    }



    private void StartBlocking()
    {
        currentState = BlockState.Blocking;
        isBlock = true;
        animatorController.PlayGuard(); // 방어 애니메이션 재생 
    }

    private void StopBlocking()
    {
        currentState = BlockState.Idle;
        isBlock = false;

        if (playerMovement != null && animatorController != null)
        {
            string lastDir = playerMovement.lastDirection;
            animatorController.PlayIdle(lastDir); // 마지막 방향으로 Idle 재생
        }
    }


    private void EnterExhausted()
    {
        isExhausted = true; // 탈진 상태로 설정
        isBlock = false;
        currentState = BlockState.Exhausted;
        exhaustedTimer = 0f;

        //animatorController.PlayExhausted(); // 필요한 애니메이션 재생

        // 방어 중단 처리도 이 안에서
        if (playerMovement != null && animatorController != null)
        {
            string lastDir = playerMovement.lastDirection;
            animatorController.PlayIdle(lastDir);
        }
    }


    private void RecoverGauge(float rate, float maxLimit)
    {
        if (CurrentGauge < maxLimit)
        {
            CurrentGauge += rate * Time.deltaTime;
            CurrentGauge = Mathf.Min(CurrentGauge, maxLimit);
        }
    }
}
