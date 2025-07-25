using UnityEngine;
using UnityEngine.Rendering;

public class WeaponClose : WeaponBase
{
    [Header("공격 포인트")]
    public Transform FrontAttackPoint;
    public Transform BackAttackPoint;
    public Transform LeftAttackPoint;
    public Transform RightAttackPoint;

    private EnemyFSM fsm;
    private BaseMonster baseMonster;

    private void Awake()
    {
        fsm = GetComponent<EnemyFSM>();
        baseMonster = GetComponent<BaseMonster>();
    }

    public override void OnAttack()
    {
        baseMonster.EnterAttackMode();
        Vector3 dir = fsm.target.position - transform.position;
        string attackDir = GetAttackDirection(dir);

        Transform spawnPoint = null;

        switch (attackDir)
        {
            case "Front":
                spawnPoint = FrontAttackPoint;
                break;
            case "Back":
                spawnPoint = BackAttackPoint;
                break;
            case "Left":
                spawnPoint = LeftAttackPoint;
                break;
            case "Right":
                spawnPoint = RightAttackPoint;
                break;
        }

        if (spawnPoint != null)
        {
            GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
            clone.GetComponent<CloseEnemyProjectile>().Setup(fsm.target, damage, spawnPoint, attackDir, baseMonster);
        }
        StartCoroutine(baseMonster.ExitAttackMode());
    }

    protected string GetAttackDirection(Vector3 toTarget)
    {
        if (Mathf.Abs(toTarget.x) > Mathf.Abs(toTarget.y))
            return toTarget.x > 0 ? "Right" : "Left";
        else
            return toTarget.y > 0 ? "Back" : "Front";
    }
}
