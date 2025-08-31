using UnityEngine;

public class PlayBgm : MonoBehaviour
{
    [Header("----- 브금선택 -----")]
    [SerializeField] private AudioManager.Bgm _bgm;
    void Start()
    {
        AudioManager.Instance.PlayBgm(_bgm);
    }
}
