using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("----- 플레이어 -----")]
    [SerializeField] private GameObject _player;

    public GameObject Player => _player;

    private void Awake()
    {
        Instance = this;
    }
}
