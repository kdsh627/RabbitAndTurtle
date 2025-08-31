using Manager;
using TMPro;
using UnityEngine;

public class BossUI : MonoBehaviour
{
    [Header("----- 모델 -----")]
    [SerializeField] private WaveManager _waveManager;

    [Header("----- 뷰 -----")]
    [SerializeField] private TMP_Text _waveCountText;
    [SerializeField] private TMP_Text _waveTimeText;

    [Header("----- 캔버스 -----")]
    [SerializeField] private Canvas _canvas;

    [Header("----- 버튼 -----")]
    [SerializeField] private Button _menu;

    [Header("----- 웨이브 시작 시 효과 -----")]
    [SerializeField] private GameObject _startUI;

    private void Awake()
    {
        _canvas.worldCamera = CameraManager.Instance.UICamera;
        _canvas.sortingLayerID = SortingLayer.NameToID("UI");
    }

    private void OnEnable()
    {
        _waveManager.OnWaveStart += WaveStart;
        _waveManager.WaveValueChanged += UpdateView;
        _menu.onClick.AddListener(UIEventHandler.ToggleSettingUI_Invoke);
    }
    private void OnDisable()
    {
        _waveManager.OnWaveStart -= WaveStart;
        _waveManager.WaveValueChanged -= UpdateView;
        _menu.onClick.RemoveListener(UIEventHandler.ToggleSettingUI_Invoke);
    }

    public void UpdateView()
    {
        _waveCountText.text = string.Format("웨이브 : {0:D2} / {1:D2}", _waveManager.CurrentWave, _waveManager.MaxWave);

        float time = _waveManager.WaveTime;
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliSeconds = Mathf.RoundToInt(time * 100) % 100;

        _waveTimeText.text = string.Format("남은시간 : {0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliSeconds);
    }

    private void WaveStart()
    {
        _startUI.SetActive(true);
    }

}
