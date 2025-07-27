using UnityEngine;

public class WeaponStraight : WeaponBase
{
    [SerializeField] private int projectileIndex; // WeaponPool index

    public override void OnAttack()
    {
        GameObject clone = WeaponPool.Instance.Get(projectileIndex);
        if (clone == null) return;

        clone.transform.position = projectileSpawnPoint.position;
        clone.transform.rotation = Quaternion.identity;

        clone.GetComponent<EnemyProjectile>().Setup(target, damage, projectileIndex);
    }

}
