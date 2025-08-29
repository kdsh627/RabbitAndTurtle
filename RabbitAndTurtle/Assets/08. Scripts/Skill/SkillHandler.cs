using UnityEngine;
using UnityEngine.EventSystems;

public class SkillHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("응애");
        SkillManager.Instance.Skill.Monster = gameObject;
        SkillManager.Instance.Skill.SkillActive();
    }
}
