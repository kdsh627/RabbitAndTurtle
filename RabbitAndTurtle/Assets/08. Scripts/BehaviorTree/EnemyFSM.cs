using System.Linq;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    private float cooldownTime = 2f;
    [SerializeField]
    private float damage = 10f;

    public Transform target;
    private NavMeshAgent navMeshAgent;
    private BehaviorGraphAgent behavionrAgent;
    private WeaponBase currentWeapon;

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
        currentWeapon.Setup(target, damage, cooldownTime);
    }

    
}
