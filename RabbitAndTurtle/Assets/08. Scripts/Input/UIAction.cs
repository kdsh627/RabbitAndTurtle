using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIAction : MonoBehaviour
{
    /// <summary>
    /// UI 액션
    /// </summary>
    public void ActionCancel(InputAction.CallbackContext context)
    {
        //입력 시작 시 최초 실행
        if (context.started)
        {
            //UI끄는 기능 추가


            //액션 맵 변경
            InputManager.OnSwitchActionMap?.Invoke(ActionMap.Player);
        }
    }
}
