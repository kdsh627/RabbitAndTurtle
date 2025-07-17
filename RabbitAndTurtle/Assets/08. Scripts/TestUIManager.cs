using UnityEngine;
using UnityEngine.UI;

public class TestUIManager : MonoBehaviour
{

    public Slider BlockGaugeSlider;
    public PlayerBlock playerBlock; // PlayerBlock 스크립트의 인스턴스

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BlockGaugeSlider.value = playerBlock.currentGauge / playerBlock.MaxBlockTime;
    }

}
