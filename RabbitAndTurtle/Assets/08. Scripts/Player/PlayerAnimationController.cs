using UnityEngine;
using UnityEngine.Android;


public class PlayerAnimationController : MonoBehaviour
{
    private Animator ani;
    public bool isDie = false;

    public void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public void PlayIdle(string Dir)
    {
        if (isDie) return;
        ani.Play($"{Dir}Idle");
    }

    public void PlayWalk(string Dir)
    {
        if (isDie) return;
        ani.Play($"{Dir}Walk");
    }

    public void PlayGuard()
    {
        if (isDie) return;
        ani.Play("Block");
    }

    public void PlayDie()
    {
        ani.Play("Die");
    }

}
