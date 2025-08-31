using UnityEngine;
using UnityEngine.UI;

public class HowToPlayUI : ToggleUI
{
    [Header("---- 닫기 ----")]
    [SerializeField] private Button _exitButton;

    protected override void Start()
    {
        base.Start();

        UIEventHandler.ToggleHowToPlayUI += UIEvent_ToggleUI;
        _exitButton.onClick.AddListener(ButtonEvent_Exit);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        UIEventHandler.ToggleHowToPlayUI -= UIEvent_ToggleUI;
        _exitButton.onClick.RemoveAllListeners();
    }

    private void ButtonEvent_Exit()
    {
        UIEvent_ToggleUI();
    }

}
