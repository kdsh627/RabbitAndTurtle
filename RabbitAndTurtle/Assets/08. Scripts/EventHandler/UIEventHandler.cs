using System;

public class UIEventHandler
{
    //Setting UI 토글
    public static event Action ToggleSettingUI;

    //GameOver UI 토글
    public static event Action ToggleGameOverUI;

    //GameOver UI 토글
    public static event Action ToggleSkillEnforceUI;

    #region Invoke 처리
    public static void ToggleSettingUI_Invoke() => ToggleSettingUI?.Invoke();
    public static void ToggleGameOverUI_Invoke() => ToggleGameOverUI?.Invoke();

    public static void ToggleSkillEnforceUI_Invoke() => ToggleSkillEnforceUI?.Invoke();
    #endregion

}
