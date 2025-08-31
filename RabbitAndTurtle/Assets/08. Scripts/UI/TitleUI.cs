using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [Header("------ 버튼 ------")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionButton;
    [SerializeField] private Button _exitButton;

    private void OnEnable()
    {
        _startButton.onClick.AddListener(ButtonEvent_Start);
        _optionButton.onClick.AddListener(ButtonEvent_Option);
        _exitButton.onClick.AddListener(ButtonEvent_Exit);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(ButtonEvent_Start);
        _optionButton.onClick.RemoveListener(ButtonEvent_Option);
        _exitButton.onClick.RemoveListener(ButtonEvent_Exit);
    }

    private void ButtonEvent_Start()
    {
        GameEventHandler.GamePlayExcuted_Invoke();
    }

    private void ButtonEvent_Option()
    {
        UIEventHandler.ToggleSettingUI_Invoke();
    }

    private void ButtonEvent_Credit()
    {
        //
    }

    private void ButtonEvent_Exit()
    {
        GameEventHandler.ExitExcuted_Invoke();
    }
}
