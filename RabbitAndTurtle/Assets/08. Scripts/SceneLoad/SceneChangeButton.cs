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
        button.onClick.AddListener(SceneEventHandler.GameSceneLoaded.Invoke);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(SceneEventHandler.GameSceneLoaded.Invoke);
    }
}
