using UnityEngine;
using UnityEngine.Android;


public class PlayerAnimationController : MonoBehaviour
{
    private Animator ani;

    public void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public void PlayIdle(string Dir) => ani.Play($"{Dir}Idle");
    public void PlayWalk(string Dir) => ani.Play($"{Dir}Walk");
    //public void PlayGuard() => animator.Play("Guard");
}
