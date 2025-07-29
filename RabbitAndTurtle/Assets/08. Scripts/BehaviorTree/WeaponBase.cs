using System.Collections;
using Unity.Behavior.Demo;
using UnityEngine;
using UnityEngine.AI;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject projectilePrefab;
    [SerializeField]
    protected Transform projectileSpawnPoint;

    protected Transform target;

    protected float damage;
    protected float maxCooldownTime;
    private float currentCooldownTime = 0f;
    private bool isSkillAvailable = true;
    private NavMeshAgent agent;

    private void Awake()
    {
       agent = GetComponent<NavMeshAgent>();
    }
    public void Setup(Transform target, float damage, float cooldownTime)
    {
        this.target = target;
        this.damage = damage;
        maxCooldownTime = cooldownTime;
    }

    private void Update()
    {
        if(isSkillAvailable == false && Time.time - currentCooldownTime > maxCooldownTime)
        {
            isSkillAvailable = true;
        }
    }

    public void TryAttack()
    {
        if(isSkillAvailable == true)
        {
            OnAttack();
            StartCoroutine(StopWhileAttacking());
            isSkillAvailable=false;
            currentCooldownTime = Time.time;
        }
    }

    public IEnumerator StopWhileAttacking()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
    }

    public abstract void OnAttack();

}
