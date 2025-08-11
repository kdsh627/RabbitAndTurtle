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
        button.onClick.AddListener(GameEventHandler.BossClearExcuted.Invoke);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(GameEventHandler.BossClearExcuted.Invoke);
    }
}
