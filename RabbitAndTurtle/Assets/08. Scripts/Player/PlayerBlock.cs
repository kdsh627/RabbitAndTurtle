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

    public GameObject ExhaustedEff;
    public GameObject BlockCollider;
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
        ExhaustedEff.SetActive(false); // 탈진 이펙트 비활성화
        BlockCollider.SetActive(false);
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
        if (!BlockCollider.activeSelf)
            BlockCollider.SetActive(true);

        currentGauge -= Time.deltaTime;
        currentGauge = Mathf.Max(currentGauge, 0f);

        if (!isBlockButtonHeld)
        {
            StopBlocking(); // 그냥 Idle로 전환
            BlockCollider.SetActive(false);
            return;
        }

        if (currentGauge <= ExhaustThreshold)
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

        if (currentGauge >= ExhaustThreshold)
        {
            currentState = BlockState.Idle;
            ExhaustedEff.SetActive(false); // 회복 완료 후 이펙트 끄기
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
        if (currentGauge < maxLimit)
        {
            currentGauge += rate * Time.deltaTime;
            currentGauge = Mathf.Min(currentGauge, maxLimit);
        }
    }
}
