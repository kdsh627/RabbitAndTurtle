using UnityEngine;
using UnityEngine.UI;

public class ClearSceneButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(GameEventHandler.TitleExcuted_Invoke);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(GameEventHandler.TitleExcuted_Invoke);
    }
}
