using UnityEngine;
using UnityEngine.UI;

public class SkillEnforceUI : ToggleUI
{
    [Header("--- 스킬 버튼 ---")]
    [SerializeField] private Button _whaleToss;
    [SerializeField] private Button _fishAttack;
    [SerializeField] private Button _shieldRotation;

    [Header("--- 스킬 ---")]
    [SerializeField] private PlayerSkill _skill1;
    [SerializeField] private ShieldRotate _skill2;

    [Header("--- 스킬 막기 ---")]
    [SerializeField] private GameObject _skill1Max;
    [SerializeField] private GameObject _skill2Max;


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
        _skill1.Levelup();

        if (_skill1.isMaxLevel())
        {
            _whaleToss.interactable = false;
            _skill1Max.SetActive(true);
        }
        UIEvent_ToggleUI();
    }

    private void ButtonEvent_FishAttack()
    {
        _skill2.Levelup();

        if (_skill2.isMaxLevel())
        {
            _fishAttack.interactable = false;
            _skill1Max.SetActive(true);
        }

        UIEvent_ToggleUI();
    }

    private void ButtonEvent_ShieldRotation()
    {
        UIEvent_ToggleUI();
    }

}
