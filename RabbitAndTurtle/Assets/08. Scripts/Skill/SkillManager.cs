using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; } 

    [Header("----- 스킬 -----")]
    [SerializeField] private FlyingFish _skill;

    public FlyingFish Skill => _skill;

    private void Awake()
    {
        Instance = this;
    }
}
