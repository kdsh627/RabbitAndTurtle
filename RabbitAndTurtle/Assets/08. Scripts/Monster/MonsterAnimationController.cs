using UnityEngine;
using UnityEngine.Android;


public class MonsterAnimationController : MonoBehaviour
{
    private Animator ani;

    public void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public void PlayIdle(string Dir) => ani.Play($"{Dir}Idle");
    public void PlayWalk(string Dir) => ani.Play($"{Dir}Walk");

    public void PlayHurt() => ani.Play("Hurt");
}
