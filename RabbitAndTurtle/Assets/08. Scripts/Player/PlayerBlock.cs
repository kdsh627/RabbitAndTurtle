using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBlock : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAnimationController animatorController;

    [Header("방어 변수")]
    public float MaxBlockTime = 0.5f;           // 최대 방어 시간
    public float ExhaustThreshold = 0.4f;       // 빨간선 임계값
    public float RecoveryRate = 0.5f;           // Idle 회복 속도
    public float ExhaustedDelay = 1f;           // 탈진 시 회복 지연 시간

    private enum BlockState
    {
        Idle,
        Blocking,
        Exhausted
    }

    private BlockState currentState = BlockState.Idle;
    public float currentGauge;

    private float exhaustedTimer = 0f; // 탈진 대기 타이머

    private InputManager inputManager;

    private bool isBlockButtonHeld = false;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animatorController = GetComponent<PlayerAnimationController>();
        currentGauge = MaxBlockTime;
    }

    void Update()
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

    public void SetBlocking(bool isHeld)
    {
        isBlockButtonHeld = isHeld;
    }


    private void HandleIdle()
    {
        if (isBlockButtonHeld && currentGauge > ExhaustThreshold)
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
        currentGauge -= Time.deltaTime;
        currentGauge = Mathf.Max(currentGauge, 0f);

        if (currentGauge <= ExhaustThreshold)
        {
            StopBlocking();
            EnterExhausted();
            return;
        }

        if (!isBlockButtonHeld)
        {
            StopBlocking();
        }
    }


    private void HandleExhausted()
    {
        if (exhaustedTimer < ExhaustedDelay)
        {
            exhaustedTimer += Time.deltaTime;
            return; // 대기 중에는 회복 안 함
        }

        // 대기 끝나면 빨간선까지 회복
        RecoverGauge(RecoveryRate, ExhaustThreshold);

        if (currentGauge >= ExhaustThreshold)
        {
            currentState = BlockState.Idle;
        }
    }

    private void StartBlocking()
    {
       currentState = BlockState.Blocking;
       animatorController.PlayGuard(); // 방어 애니메이션 재생 
    }

    private void StopBlocking()
    {
        currentState = BlockState.Idle;

        if (playerMovement != null && animatorController != null)
        {
            string lastDir = playerMovement.lastDirection;
            animatorController.PlayIdle(lastDir); // 마지막 방향으로 Idle 재생
        }
    }


    private void EnterExhausted()
    {
        currentState = BlockState.Exhausted;
        exhaustedTimer = 0f;
        // Animator.SetTrigger("Exhausted");
    }

    private void RecoverGauge(float rate, float maxLimit)
    {
        if (currentGauge < maxLimit)
        {
            currentGauge += rate * Time.deltaTime;
            currentGauge = Mathf.Min(currentGauge, maxLimit);
        }
    }
}
