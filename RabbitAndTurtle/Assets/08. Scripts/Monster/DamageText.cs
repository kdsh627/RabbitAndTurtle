using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float duration = 1f;
    [SerializeField]
    private AnimationCurve alphaCurve =
        AnimationCurve.EaseInOut(0f, 1f, 1f, 0f); // 시간에 따른 알파

    [SerializeField] private Color lowColor = Color.white;           
    [SerializeField] private Color highColor = new Color(1f, 0f, 0f); 


    private float elapsed;
    private float minDamage;
    private float maxDamage;

    // 한 번에 세팅
    public void Setup(float damage, float minDmg, float maxDmg)
    {
        Init(minDmg, maxDmg);
        SetText(damage);
        elapsed = 0f;
    }

    public void Init(float minDmg, float maxDmg)
    {
        minDamage = minDmg;
        maxDamage = maxDmg;
    }

    public void SetText(float damage)
    {
        if (damageText == null) return;

        damageText.text = Mathf.RoundToInt(damage).ToString();

        // 0~1 정규화
        float t = Mathf.InverseLerp(minDamage, maxDamage, damage);
        damageText.color = Color.Lerp(lowColor, highColor, t);
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        // 위로 부드럽게 떠오르게
        transform.position += Vector3.up * (speed * Time.deltaTime);

        // 알파 페이드
        if (damageText != null)
        {
            Color c = damageText.color;
            c.a = alphaCurve.Evaluate(Mathf.Clamp01(elapsed / duration));
            damageText.color = c;
        }

        if (elapsed >= duration)
            Destroy(gameObject);
    }
}
