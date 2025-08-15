using System;
using DG.Tweening;
using Effect;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : ToggleUI
{
    [Header("--- 페이드에 필요한 변수 ---")]
    [SerializeField] private Image _image;
    [SerializeField] private float _duration;

    private void Awake()
    {
        SceneEventHandler.SceneFadeIn += UIEvent_FadeIn;
        SceneEventHandler.SceneFadeOut += UIEvent_FadeOut;

        Material instancedMat = Instantiate(_image.material);

        _image.material = instancedMat;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        SceneEventHandler.SceneFadeIn -= UIEvent_FadeIn;
        SceneEventHandler.SceneFadeOut -= UIEvent_FadeOut;
    }

    private Tween UIEvent_WipeFadeIn()
    {
        _image.material.SetFloat("_Progress", 0.0f);
        return FadeEffect.WipeFadeIn(_image.material, _duration, false, 2.0f, () =>
        {
            UIEvent_ToggleUI();
        });
    }

    private Tween UIEvent_WipeFadeOut()
    {
        UIEvent_ToggleUI();

        _image.material.SetFloat("_Progress", 1.0f);
        return FadeEffect.WipeFadeOut(_image.material, _duration, false, 0.0f);
    }

    private Tween UIEvent_FadeIn()
    {
        Debug.Log("페이드 인");
        _image.material.SetFloat("_Progress", 0.0f);
        _image.material.SetFloat("_IsFadeIn", 1.0f);
        Color color = _image.color;

        color.a = 1.0f;

        return FadeEffect.FadeIn(_image, 2.0f, () =>
        {
            UIEvent_ToggleUI();
        });
    }

    private Tween UIEvent_FadeOut()
    {
        Debug.Log("페이드 아웃");
        _image.material.SetFloat("_Progress", 1.0f);
        _image.material.SetFloat("_IsFadeIn", 0.0f);
        Color color = _image.color;
        color.a = 0.0f;
        _image.color = color;

        UIEvent_ToggleUI();

        return FadeEffect.FadeOut(_image, 2.0f);
    }
}