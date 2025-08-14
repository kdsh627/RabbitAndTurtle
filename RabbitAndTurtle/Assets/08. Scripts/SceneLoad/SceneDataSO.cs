using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StateScene
{
    public string CoreScene;
    public List<string> SubScenePaths;
}

[CreateAssetMenu(fileName = "SceneDataSO", menuName = "Scriptable Objects/SceneDataSO")]
public class SceneDataSO : ScriptableObject
{
    public string LoadingScene;

    public string TitleScene;

    public string ClearScene;

    public StateScene WaveScene;

    public StateScene BossScene;
}