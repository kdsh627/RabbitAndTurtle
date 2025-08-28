using UnityEngine;
using UnityEngine.InputSystem;

public class TargetCursor : MonoBehaviour
{
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
