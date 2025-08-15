using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("----- 플레이어 -----")]
    [SerializeField] private GameObject _player;

    [Header("----- 레벨 데이터 -----")]
    [SerializeField] private LevelDataSO _levelData;

    public GameObject Player => _player;
    public LevelDataSO LevelData => _levelData;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _levelData.Init();
    }
}
