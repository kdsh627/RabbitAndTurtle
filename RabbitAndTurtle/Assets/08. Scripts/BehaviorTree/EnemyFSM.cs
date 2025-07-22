using System.Linq;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent navMeshAgent;
    private BehaviorGraphAgent behavionrAgent;

    public void Setup(Transform target)
    {
        this.target = target;
        navMeshAgent = GetComponent<NavMeshAgent>();
        behavionrAgent = GetComponent<BehaviorGraphAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        //behavionrAgent.SetVariableValue("PatrolPoints", wayPoints.ToList());
        behavionrAgent.SetVariableValue("Target", target.gameObject);
    }

    private void Update()
    {
        navMeshAgent.SetDestination(target.position);
    }
}
