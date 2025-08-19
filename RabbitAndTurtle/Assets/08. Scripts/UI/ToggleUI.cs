using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    [Header("------ UI ------")]
    [SerializeField] private string _name;
    [SerializeField] private GameObject _ui;

    public string Name => _name;
    public GameObject UI => _ui;
    private void Awake()
    {
        _ui.SetActive(false);
    }

    protected virtual void Start()
    {
        UIManager.Instance.AddUIDictionary(_name, _ui);
    }

    protected virtual void OnDestroy()
    {
        UIManager.Instance.RemoveUIDictionary(_name);
    }

    protected void UIEvent_ToggleUI()
    {
        UIManager.Instance.ToggleUI(name, true);
    }
}
