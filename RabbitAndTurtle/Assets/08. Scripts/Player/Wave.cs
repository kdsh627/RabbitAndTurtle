using UnityEngine;

public class Wave : MonoBehaviour
{
    public float damage = 10f; // 데미지 값
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 0.5f); // 2초 후 오브젝트 파괴
    }
}
