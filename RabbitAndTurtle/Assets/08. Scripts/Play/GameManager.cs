using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("--- 스테이지 씬 ---")]
    [SerializeField] string[] _stageScenePaths;

    private void Awake()
    {
        SceneEventHandler.SceneLoadedAdditivelyByPath(_stageScenePaths[0]);
    }
}
