using System.Collections;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void QSkill()
    {
        StartCoroutine(WhaleThrow());
    }

    void WSkill()
    {
        StartCoroutine(FlyFishShot());
    }

    void ESkill()
    {
        StartCoroutine(TurtleShield());
    }

    IEnumerator WhaleThrow()
    {
        yield return null;
    }

    IEnumerator FlyFishShot()
    {
        yield return null;
    }

    IEnumerator TurtleShield()
    {
        yield return null;
    }
}
