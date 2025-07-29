using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Sprite[] SongpeyonSprites;
    private MovementRigidbody2D movement;
    private Transform target;
    public float damage;
    public bool isReflected;

    private int poolIndex;

    public void Setup(Transform target, float damage, int poolIndex)
    {
        this.poolIndex = poolIndex;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (SongpeyonSprites.Length > 0 && sr != null)
        {
            sr.sprite = SongpeyonSprites[Random.Range(0, SongpeyonSprites.Length)];
        }

        movement = GetComponent<MovementRigidbody2D>();
        this.target = target;
        this.damage = damage;

        movement.MoveTo((target.position - transform.position).normalized);

        isReflected = false;
        Invoke(nameof(ReturnToPool), 3f); // 3초 후 복귀
    }

    private void ReflectProjectile()
    {
        if (movement != null)
        {
            movement.Reflect(); // 반사 방향 전환
        }
    }

    private void ReturnToPool()
    {
        WeaponPool.Instance.Return(poolIndex, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlockCollider"))
        {
            isReflected = true;
            movement.Reflect();
        }
    }
}
