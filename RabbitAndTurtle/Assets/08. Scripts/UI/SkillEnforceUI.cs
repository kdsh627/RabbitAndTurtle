using UnityEngine;
using UnityEngine.UI;

public class SkillEnforceUI : ToggleUI
{
    [Header("--- 스킬 버튼 ---")]
    [SerializeField] private Button _whaleToss;
    [SerializeField] private Button _fishAttack;
    [SerializeField] private Button _shieldRotation;

    protected override void Start()
    {
        base.Start();

        UIEventHandler.ToggleSkillEnforceUI += UIEvent_ToggleUI;
        _whaleToss.onClick.AddListener(ButtonEvent_WhaleTossEnforce);
        _fishAttack.onClick.AddListener(ButtonEvent_FishAttack);
        _shieldRotation.onClick.AddListener(ButtonEvent_ShieldRotation);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        UIEventHandler.ToggleSkillEnforceUI -= UIEvent_ToggleUI;
        _whaleToss.onClick.RemoveListener(ButtonEvent_WhaleTossEnforce);
        _fishAttack.onClick.RemoveListener(ButtonEvent_FishAttack);
        _shieldRotation.onClick.RemoveListener(ButtonEvent_ShieldRotation);
    }

    private void ButtonEvent_WhaleTossEnforce()
    {
        UIEvent_ToggleUI();
    }

    private void ButtonEvent_FishAttack()
    {
        UIEvent_ToggleUI();
    }

    private void ButtonEvent_ShieldRotation()
    {
        UIEvent_ToggleUI();
    }

}
