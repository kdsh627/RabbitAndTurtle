using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [Header("------ 버튼 ------")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionButton;
    [SerializeField] private Button _howTosPlayButton;
    [SerializeField] private Button _exitButton;

    private void OnEnable()
    {
        _startButton.onClick.AddListener(ButtonEvent_Start);
        _optionButton.onClick.AddListener(ButtonEvent_Option);
        _howTosPlayButton.onClick.AddListener(ButtonEvent_HowToPlay);
        _exitButton.onClick.AddListener(ButtonEvent_Exit);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(ButtonEvent_Start);
        _optionButton.onClick.RemoveListener(ButtonEvent_Option);
        _howTosPlayButton.onClick.RemoveListener(ButtonEvent_HowToPlay);
        _exitButton.onClick.RemoveListener(ButtonEvent_Exit);
    }

    private void ButtonEvent_Start()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.ButtonClick);
        GameEventHandler.GamePlayExcuted_Invoke();
    }

    private void ButtonEvent_Option()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.ButtonClick);
        UIEventHandler.ToggleSettingUI_Invoke();
    }
    private void ButtonEvent_HowToPlay()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.ButtonClick);
        UIEventHandler.ToggleHowToPlayUI_Invoke();
    }

    private void ButtonEvent_Exit()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.ButtonClick);
        GameEventHandler.ExitExcuted_Invoke();
    }
}
