using UnityEngine;
using UnityEngine.Tilemaps;

public class LowerAlpha : MonoBehaviour
{
    public Tilemap spriteA;
    public Tilemap spriteB;

    [Range(0f, 1f)] public float inAlpha = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            SetAlphaBoth(inAlpha);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            SetAlphaBoth(1f);
    }

    void SetAlphaBoth(float a)
    {
        if (spriteA) { var c = spriteA.color; c.a = a; spriteA.color = c; }
        if (spriteB) { var c = spriteB.color; c.a = a; spriteB.color = c; }
    }
}
