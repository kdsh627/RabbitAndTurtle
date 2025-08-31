using Unity.VisualScripting;
using UnityEngine;

public class PlayBgm : MonoBehaviour
{
    [Header("----- 브금선택 -----")]
    [SerializeField] private AudioManager.Bgm _bgm;
    void Start()
    {
        AudioManager.Instance.PlayBgm(_bgm);
        SceneEventHandler.SceneExited += AudioManager.Instance.StopBgm;
    }

    void OnDestroy()
    {
        SceneEventHandler.SceneExited -= AudioManager.Instance.StopBgm;
    }
}
