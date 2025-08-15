using UnityEngine;
using UnityEngine.Android;


public class PlayerAnimationController : MonoBehaviour
{
    private Animator ani;
    private PlayerStat playerStat;
    public void Awake()
    {
        ani = GetComponent<Animator>();
        playerStat = GetComponent<PlayerStat>();
    }

    public void PlayIdle(string Dir)
    {
        if (playerStat.isDie) return;
        ani.Play($"{Dir}Idle");
    }

    public void PlayWalk(string Dir)
    {
        if (playerStat.isDie) return;
        ani.Play($"{Dir}Walk");
    }

    public void PlayGuard()
    {
        if (playerStat.isDie) return;
        ani.Play("Block");
    }

    public void PlayDie()
    {
        ani.Play("Die");
    }

}
