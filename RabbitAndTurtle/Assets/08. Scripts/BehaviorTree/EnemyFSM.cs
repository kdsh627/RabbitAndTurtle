using System.Linq;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    private float cooldownTime = 2f;
    [SerializeField] private float minDamage = 6f;
    [SerializeField] private float maxDamage = 14f;

    public Transform target;
    private NavMeshAgent navMeshAgent;
    private BehaviorGraphAgent behavionrAgent;
    private WeaponBase currentWeapon;

    public float MinDamage => Mathf.Min(minDamage, maxDamage);
    public float MaxDamage => Mathf.Max(minDamage, maxDamage);

    public float GetRandomDamage()
    {
        return Random.Range(MinDamage, MaxDamage);
    }

    public void Setup(Transform target, GameObject[] wayPoints)
    {
        this.target = target;
        navMeshAgent = GetComponent<NavMeshAgent>();
        behavionrAgent = GetComponent<BehaviorGraphAgent>();
        currentWeapon = GetComponent<WeaponBase>();

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        behavionrAgent.SetVariableValue("PatrolPoints", wayPoints.ToList());
        behavionrAgent.SetVariableValue("Target", target.gameObject);
        currentWeapon.Setup(target, GetRandomDamage(), cooldownTime);
    }


}
