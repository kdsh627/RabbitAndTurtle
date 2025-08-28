using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    private GameObject _player;
    
    public GameObject Player
    {
        get { return _player; }
        set { _player = value; }
    }

    /// <summary>
    /// 플레이어 Move Action
    /// </summary>
    /// <param name="context"></param>
    public void ActionMove(InputAction.CallbackContext context)
    {
        PlayerMovement movement = _player.GetComponent<PlayerMovement>();
        Vector2 direction = context.ReadValue<Vector2>();

        // 키 누를 때 or 방향 바꿀 때
        if (context.started || context.performed)
        {
            movement.SetMoveInput(direction);
        }
        // 키 뗄 때
        else if (context.canceled)
        {
            movement.SetMoveInput(Vector2.zero);
        }
    }


    /// <summary>
    /// 플레이어 막기 액션
    /// </summary>
    /// <param name="context"></param>
    public void ActionGuard(InputAction.CallbackContext context)
    {
        PlayerBlock block = _player.GetComponent<PlayerBlock>();

        //입력 시작 시 최초 실행
        if (context.started)
        {
            //막기 액션 추가
            block.SetBlocking(true);
        }
        else if (context.canceled)
        {
            //키 입력 끝날때 추가
            block.SetBlocking(false);

        }
    }

    /// <summary>
    /// 플레이어 액티브 스킬 액션
    /// </summary>
    /// <param name="context"></param>
    public void ActionActiveSkill(InputAction.CallbackContext context)
    {
        PlayerSkill skill = _player.GetComponent<PlayerSkill>();
        //입력 시작 시 최초 실행
        if (context.started)
        {
            skill.QSkill();
        }
    }

    /// <summary>
    /// 플레이어 UI열기 액션
    /// </summary>
    public void ActionOpenUI(InputAction.CallbackContext context)
    {
        //입력 시작 시 최초 실행
        if (context.started)
        {
            //UI 토글 추가


            //액션 맵 변경
            InputManager.OnSwitchActionMap?.Invoke(ActionMap.SettingUI);
        }
    }
}
