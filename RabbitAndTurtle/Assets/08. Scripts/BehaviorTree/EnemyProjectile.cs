using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Sprite[] SongpeyonSprites;
    private MovementRigidbody2D movement;
    private Transform target;
    public float damage;
    public bool isReflected;

    public void Setup(Transform target, float damage)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (SongpeyonSprites.Length > 0 && sr != null)
        {
            sr.sprite = SongpeyonSprites[Random.Range(0, SongpeyonSprites.Length)];
        }

        movement = GetComponent<MovementRigidbody2D>();
        this.target = target;
        this.damage = damage;

        movement.MoveTo((target.position - transform.position).normalized);
    }

    private void Start()
    {
        isReflected = false;
        Destroy(gameObject, 3f); // 3초 후 자동 파괴
    }

    private void ReflectProjectile()
    {
        if (movement != null)
        {
            movement.Reflect(); // 반사 방향 전환
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("BlockCollider"))
        {
            isReflected = true;
            ReflectProjectile();
        }
    }
}
