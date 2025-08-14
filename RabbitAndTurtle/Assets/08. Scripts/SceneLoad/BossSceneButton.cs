using UnityEngine;
using UnityEngine.UI;

public class BossSceneButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ButtonEvent_BossClear);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ButtonEvent_BossClear);
    }

    private void ButtonEvent_BossClear()
    {
        GameEventHandler.BossClearExcuted_Invoke();
    }
}
