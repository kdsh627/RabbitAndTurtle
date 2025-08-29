using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [Header("---- 액티브 스킬(뷰) ---")]
    [SerializeField] private UIEffectTweener _tweener1;
    [SerializeField] private Image _coolTimeImage1;
    [SerializeField] private UIEffectTweener _tweener2;
    [SerializeField] private Image _coolTimeImage2;

    [Header("---- 모델 -----")]
    [SerializeField] private PlayerSkill playerSkill1;
    [SerializeField] private FlyingFish playerSkill2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerSkill1.OnCoolTimeChanged += OnSkill1UpdateView;
        playerSkill1.OnSkillActive += OnSkiil1Active;
        playerSkill2.OnCoolTimeChanged += OnSkill2UpdateView;
        playerSkill2.OnSkillActive += OnSkiil2Active;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        playerSkill2.OnCoolTimeChanged -= OnSkill2UpdateView;
        playerSkill2.OnSkillActive -= OnSkiil2Active;
    }

    private void OnSkill1UpdateView()
    {
        _coolTimeImage1.fillAmount = playerSkill1.CoolTime / playerSkill1.MaxCoolTime;
    }
    private void OnSkill2UpdateView()
    {
        _coolTimeImage2.fillAmount = playerSkill2.CoolTime / playerSkill2.MaxCoolTime;
    }

    private void OnSkiil1Active()
    {
        _tweener1.Play();
    }

    private void OnSkiil2Active()
    {
        _tweener2.Play();
    }
}
