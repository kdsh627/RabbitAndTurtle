using UnityEngine;
using UnityEngine.UI;

public class TestUIManager : MonoBehaviour
{
    public Slider HpSlider;
    public Slider BlockGaugeSlider;
    public PlayerBlock playerBlock; // PlayerBlock 스크립트의 인스턴스
    public PlayerStat playerStat;
    public Image FillImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   HpSlider.maxValue = playerStat.maxHealth;

        BlockGaugeSlider.minValue = 0f;
        BlockGaugeSlider.maxValue = playerBlock.MaxBlockTime;
    }

    void Update()
    {
        BlockGaugeSlider.value = playerBlock.currentGauge;
        HpSlider.value = playerStat.currentHealth;
        float ratio = playerBlock.currentGauge / playerBlock.MaxBlockTime;
        FillImage.color = Color.Lerp(Color.red, Color.green, ratio);
    }



}
