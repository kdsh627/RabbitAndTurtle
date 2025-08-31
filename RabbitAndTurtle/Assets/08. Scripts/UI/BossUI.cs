using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [Header("----- 모델 -----")]
    [SerializeField] private Boss _boss;
    [SerializeField] private BossManager _bossManager;

    [Header("----- 뷰 -----")]
    [SerializeField] private Image _bossBar;
    [SerializeField] private TMP_Text _bossHP;

    [Header("----- 캔버스 -----")]
    [SerializeField] private Canvas _canvas;

    [Header("----- 버튼 -----")]
    [SerializeField] private Button _menu;

    [Header("----- 보스 시작 시 효과 -----")]
    [SerializeField] private GameObject _startUI;

    private void Awake()
    {
        _canvas.worldCamera = CameraManager.Instance.UICamera;
        _canvas.sortingLayerID = SortingLayer.NameToID("UI");
    }

    private void OnEnable()
    {
        _bossManager.OnBossStart += BossStart;
        _menu.onClick.AddListener(UIEventHandler.ToggleSettingUI_Invoke);
    }
    private void OnDisable()
    {
        _bossManager.OnBossStart -= BossStart;
        _menu.onClick.RemoveListener(UIEventHandler.ToggleSettingUI_Invoke);
    }

    public void UpdateView()
    {
        _bossHP.text = string.Format("{0}", _bossHP);
        _bossBar.fillAmount = _boss.HP / _boss.MaxHP;
    }
    private void BossStart()
    {
        _startUI.SetActive(true);
    }
}
