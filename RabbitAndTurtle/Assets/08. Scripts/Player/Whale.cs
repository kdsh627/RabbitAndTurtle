using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject effectPrefab; // 생성할 이펙트 프리팹

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f); // 2초 후 오브젝트 파괴
    }

    // 파괴 직전에 호출되는 메서드
    private void OnDestroy()
    {
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }
    }
}
