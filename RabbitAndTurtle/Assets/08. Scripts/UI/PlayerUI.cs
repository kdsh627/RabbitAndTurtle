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

    [Header("----- 모델 -----")]
    [SerializeField] private PlayerStat _playerStat;
    [SerializeField] private PlayerBlock _shieldStat;

    void Start()
    {
        
    }

    private void UpdateView()
    {
        //_levelText.text = _playerStat.Level;

        float hp = _playerStat.currentHealth / _playerStat.maxHealth;
        _hpBar.fillAmount = hp;

        float shield = _shieldStat.currentGauge / _shieldStat.MaxBlockTime;
        _shieldBar.fillAmount = shield;

        //float exp = _playerStat.currentExp / _playerStat.maxExp;

        //if (_expBar.fillAmount != exp)
        //{
        //    _expBar.fillAmount = exp;
        //}
    }
}
