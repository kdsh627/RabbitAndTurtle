using UnityEngine;

public class PlayBgm : MonoBehaviour
{
    [Header("----- 브금선택 -----")]
    [SerializeField] private AudioManager.Bgm _bgm;
    void Start()
    {
        AudioManager.Instance.PlayBgm(_bgm);
    }

    private void OnDestroy()
    {
        AudioManager.Instance.StopBgm();
    }
}
