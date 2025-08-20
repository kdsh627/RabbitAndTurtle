using UnityEngine;

public class SceneDataManager : MonoBehaviour
{
    public static SceneDataManager Instance { get; private set; }

    [SerializeField] private SceneDataSO sceneData;

    public string LoadingScene => sceneData.LoadingScene;
    public string TitleScene => sceneData.TitleScene;
    public string BossCoreScene => sceneData.BossScene.CoreScene;
    public string WaveCoreScene => sceneData.WaveScene.CoreScene;
    public string ClearScene => sceneData.ClearScene;

    public string GetWaveSubScene(int index)
    {
        return sceneData.WaveScene.SubScenePaths[index];
    }

    public int GetWaveSubSceneCount()
    {
        return sceneData.WaveScene.SubScenePaths.Count;
    }

    public string GetBossSubScene(int index)
    {
        return sceneData.BossScene.SubScenePaths[index];
    }

    public int GetBossSubSceneCount()
    {
        return sceneData.BossScene.SubScenePaths.Count;
    }

    private void Awake()
    {
        Instance = this;
    }
}