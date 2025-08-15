using UnityEngine;

public class SettingUI : ToggleUI
{
    protected override void Start()
    {
        base.Start();
        UIEventHandler.ToggleSettingUI += UIEvent_ToggleUI;        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UIEventHandler.ToggleSettingUI -= UIEvent_ToggleUI;
    }
}
