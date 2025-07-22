using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButton2 : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(GameEventHandler.Clear.Invoke);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(GameEventHandler.Clear.Invoke);
    }
}
