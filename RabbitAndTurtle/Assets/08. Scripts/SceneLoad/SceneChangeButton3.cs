using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButton3 : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(GameEventHandler.LoadTitle.Invoke);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(GameEventHandler.LoadTitle.Invoke);
    }
}
