using UnityEngine;

public class Wave : MonoBehaviour
{
    public float damage = 10f; // 데미지 값
                               // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void ApplyMultiplier(float mult)
    {
        damage *= mult;
        transform.localScale *= mult; // 이펙트 시각 크기까지 함께 증가
    }

    void Start()
    {
        Destroy(gameObject, 0.5f); // 2초 후 오브젝트 파괴
    }
}
