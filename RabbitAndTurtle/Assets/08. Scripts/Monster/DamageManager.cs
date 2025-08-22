using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Canvas worldCanvas;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    // 범위까지 명시해서 호출
    public void Show(float damage, Vector3 position, float minDamage, float maxDamage)
    {
        var go = Instantiate(damageTextPrefab, worldCanvas.transform);
        go.transform.position = position;

        var dt = go.GetComponent<DamageText>();
        dt.Setup(damage, minDamage, maxDamage);
    }
}
