using System;
using System.Collections.Generic;
using Manager;
using State.SceneState;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
struct Resolution
{
    public int width;
    public int height;

    public Resolution(int _width, int _height)
    {
        width = _width;
        height = _height;
    }
}


public class SettingUI : ToggleUI
{
    [Header("--- 버튼 ---")]
    [SerializeField] private Button _exitButton; //옵션 닫기 버튼
    [SerializeField] private Button _titleButton; //타이틀 버튼

    [Header("--- 사운드 조절 ---")]
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _bgmSlider;

    [Header("--- 해상도 조절 ---")]
    [SerializeField] private Dropdown _resolutionDropdown;
    [SerializeField] private List<Resolution> _resolutionList;
    [SerializeField] private int resolutionNum;
    [SerializeField] private Toggle _screenToggle;
    [SerializeField] private FullScreenMode _screenMode;

    private List<Resolution> _resolutions = new List<Resolution>();
    private int width = 1920;
    private int height = 1080;

    private void InitResolution()
    {
        foreach (Dropdown.OptionData option in _resolutionDropdown.options)
        {
            string[] parts = option.text.Split(" x ");

            Resolution resolution = new Resolution(int.Parse(parts[0]), int.Parse(parts[1]));
            _resolutionList.Add(resolution);
        }
    }

    public void ResolutionChange(int x)
    {
        if (_resolutionList[x].width != width || _resolutionList[x].height != height)
        {
            width = _resolutionList[x].width;
            height = _resolutionList[x].height;

            Screen.SetResolution(width, height, _screenMode);
        }
    }

    public void ScreenModeChange(bool isFull)
    {
        _screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(width, height, _screenMode);
    }

    private void Awake()
    {
        UIEventHandler.ToggleSettingUI += UIEvent_ToggleUI;
    }

    protected override void Start()
    {
        base.Start();

        _sfxSlider.value = AudioManager.Instance.VolumData.SfxVolume;
        _bgmSlider.value = AudioManager.Instance.VolumData.BgmVolume;

        InitResolution();

        _screenToggle.onValueChanged.AddListener(ScreenModeChange);
        _resolutionDropdown.onValueChanged.AddListener(ResolutionChange);
        _exitButton.onClick.AddListener(ButtonEvent_SettingUI);
        _titleButton.onClick.AddListener(ButtonEvent_TitleButtonEvent);
        _sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        _bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _screenToggle.onValueChanged.RemoveListener(ScreenModeChange);
        _resolutionDropdown.onValueChanged.RemoveListener(ResolutionChange);
        _exitButton.onClick.RemoveListener(ButtonEvent_SettingUI);
        _titleButton.onClick.RemoveListener(ButtonEvent_TitleButtonEvent);
        _sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
        _bgmSlider.onValueChanged.RemoveListener(OnBgmSliderChanged);

        UIEventHandler.ToggleSettingUI -= UIEvent_ToggleUI;
    }


    private void ButtonEvent_SettingUI()
    {
        PlayClickSound();
        UIEvent_ToggleUI();
    }

    private void ButtonEvent_TitleButtonEvent()
    {
        PlayClickSound();
        if (SceneStateManager.Instance.NextSceneState.StateType == SceneState.Title)
        {
            UIEvent_ToggleUI();
        }
        else
        {
            GameEventHandler.TitleExcuted_Invoke();
        }
    }

    private void PlayClickSound()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click);
    }

    private void OnSfxSliderChanged(float value)
    {
        SoundEventHandler.OnUpdateSfxVolmue_Invoke(value);
    }

    private void OnBgmSliderChanged(float value)
    {
        SoundEventHandler.OnUpdateBgmVolmue_Invoke(value);
    }
}
