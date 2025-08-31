using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("----- 뷰 -----")]
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _shieldBar;
    [SerializeField] private Image _expBar;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _expText;

    [Header("----- 모델 -----")]
    [SerializeField] private PlayerStat _playerStat;
    [SerializeField] private PlayerBlock _shieldStat;
    [SerializeField] private LevelDataSO _levelData;

    private void Awake()
    {
        _levelData.valueChanged += UpdateView;
        _playerStat.ValueChanged += UpdateView;
        _shieldStat.ValueChanged += UpdateView;
    }

    private void OnDestroy()
    {
        _levelData.valueChanged -= UpdateView;
        _playerStat.ValueChanged -= UpdateView;
        _shieldStat.ValueChanged -= UpdateView;
    }

    private void UpdateView()
    {
        _levelText.text = _levelData.Level.ToString();

        float hp = _playerStat.CurrentHealth / _playerStat.maxHealth;
        _hpBar.fillAmount = hp;
        _hpText.text = string.Format("{0}/{1} ", (int)_playerStat.CurrentHealth, (int)_playerStat.maxHealth);

        float shield = _shieldStat.CurrentGauge / _shieldStat.MaxBlockTime;
        _shieldBar.fillAmount = shield;

        float exp = _levelData.Exp / (float)_levelData.MaxExp;
        _expBar.fillAmount = exp;
        _expText.text = string.Format("{0}/{1}", _levelData.Exp, _levelData.MaxExp);
    }
}
