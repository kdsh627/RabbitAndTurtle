using UnityEngine;
using UnityEngine.UI;

public class WaveSceneButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(GameEventHandler.WaveClearExcuted_Invoke);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(GameEventHandler.WaveClearExcuted_Invoke);
    }
}
