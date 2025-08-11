using System.Text;
using TMPro;
using UnityEngine;


public class WaveUI : MonoBehaviour
{
    [Header("모델")]
    [SerializeField] private WaveManager _waveManager;

    [Header("뷰")]
    [SerializeField] private TMP_Text _waveCountText;
    [SerializeField] private TMP_Text _waveTimeText;
    [SerializeField] private TMP_Text _maxWaveText;

    private void OnEnable()
    {
        _waveManager.WaveValueChanged += UpdateView;
    }
    private void OnDisable()
    {
        _waveManager.WaveValueChanged -= UpdateView;
    }

    public void UpdateView()
    {
        _waveCountText.text = string.Format("현재 웨이브 : {0:D2}", _waveManager.CurrentWave);

        float time = _waveManager.WaveTime;
        int minutes = (int)(time/ 60);
        int seconds = (int)(time % 60);
        int milliSeconds = Mathf.RoundToInt(time * 100) % 100;

        _waveTimeText.text = string.Format("남은시간 : {0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliSeconds);

        _maxWaveText.text = string.Format("총 웨이브 : {0:D2}", _waveManager.MaxWave);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
