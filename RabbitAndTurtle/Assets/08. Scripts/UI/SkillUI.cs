using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [Header("---- 액티브 스킬(뷰) ---")]
    [SerializeField] private UIEffectTweener _tweener;
    [SerializeField] private Image _coolTimeImage;

    [Header("---- 모델 -----")]
    [SerializeField] private PlayerSkill playerSkill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerSkill.OnCoolTimeChanged += OnUpdateView;
        playerSkill.OnSkillActive += OnSkiilActive;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        playerSkill.OnCoolTimeChanged -= OnUpdateView;
        playerSkill.OnSkillActive -= OnSkiilActive;
    }

    private void OnUpdateView()
    {
        _coolTimeImage.fillAmount = playerSkill.CoolTime / playerSkill.MaxCoolTime;
    }

    private void OnSkiilActive()
    {
        _tweener.Play();
    }
}
