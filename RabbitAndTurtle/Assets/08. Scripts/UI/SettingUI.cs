using UnityEngine;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private string _name = "SettingUI";
    [SerializeField] private GameObject _ui;

    private void Start()
    {
        UIManager.Instance.AddUIDictionary(_name, _ui);
        UIEventHandler.ToggleSettingUI += UIEvent_ToggleSettingUI;
    }

    private void UIEvent_ToggleSettingUI()
    {
        UIManager.Instance.ToggleUI(name, false);
    }

    private void OnDestroy()
    {
        UIManager.Instance.RemoveUIDictionary(_name);
        UIEventHandler.ToggleSettingUI -= UIEvent_ToggleSettingUI;
    }
}
