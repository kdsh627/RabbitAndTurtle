using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(GameEventHandler.GamePlayExcuted_Invoke);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(GameEventHandler.GamePlayExcuted_Invoke);
    }
}
