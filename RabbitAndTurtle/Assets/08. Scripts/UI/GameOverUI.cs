using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : ToggleUI
{
    [Header("----- 버튼 -----")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _goTitleButton;

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(ButtonEvent_ReStart);
        _goTitleButton.onClick.AddListener(ButtonEvent_GoTitle);
    }
    protected override void Start()
    {
        base.Start();
        UIEventHandler.ToggleGameOverUI += UIEvent_ToggleUI;
    }
    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(ButtonEvent_ReStart);
        _goTitleButton.onClick.RemoveListener(ButtonEvent_GoTitle);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UIEventHandler.ToggleGameOverUI -= UIEvent_ToggleUI;
    }

    private void ButtonEvent_ReStart()
    {
        GameEventHandler.GamePlayExcuted?.Invoke();
    }

    private void ButtonEvent_GoTitle()
    {
        GameEventHandler.TitleExcuted?.Invoke();
    }
}
