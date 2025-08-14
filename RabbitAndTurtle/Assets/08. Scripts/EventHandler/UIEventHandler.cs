using System;
using UnityEngine;

public class UIEventHandler
{
    //Setting UI 토글
    public static event Action ToggleSettingUI;
    public static void ToggleSettingUI_Invoke() => ToggleSettingUI?.Invoke();

    //GameOver UI 토글
    public static event Action ToggleGameOverUI;
    public static void ToggleGameOverUI_Invoke() => ToggleGameOverUI?.Invoke();

}
